using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Labscape.Items;
using Labscape.Data;
using Object = UnityEngine.Object;

/// <summary>
/// 아이템 정의·사용 로직·카운트 관리 전담.
/// JSON 저장·로드는 DataManager, 도메인 로직은 여기서 처리.
/// </summary>
public class ItemManager
    {
        // 싱글톤을 통해 다른 매니저들에 접근
        private ItemDatabase _itemDatabase;

        // --- SO 기반 사용 로직 매핑 ---
        Dictionary<ItemType, IItemUse> _itemUseDict;

        // --- 버프·쿨다운 관리 ---
        Dictionary<ItemType, float> _buffEndTimes = new Dictionary<ItemType, float>();
        Dictionary<ItemType, float> _nextAvailableTime = new Dictionary<ItemType, float>();

        // 아이템 수량 변경 이벤트 (itemId, newCount)
        public event Action<ItemType, int> OnItemCountChanged;

        // 메모리 누수 방지를 위해 핸들러를 필드로 보관
        private Action<SaveData> _onDataLoadedHandler;

        /// <summary>
        /// ItemDatabase 인스턴스 반환 (UI에서 아이템 정보 조회용)
        /// </summary>
        public ItemDatabase GetItemDatabase()
        {
            return _itemDatabase;
        }
        /// <summary>
        /// ItemManager 초기화
        /// </summary>
        public void Init()
        {
            Debug.Log("ItemManager: Initialize");
            
            // ItemDatabase 직접 생성 및 초기화
            InitializeItemDatabase();
            
            // 1) SO 로직 로드
            LoadItemUses();

            // 1) DataManager.OnLoaded 핸들러 생성 & 구독
            _onDataLoadedHandler = save =>
            {
                if (_itemDatabase != null)
                {
                    foreach (var type in _itemDatabase.GetAllItemDefinitions().Keys)
                        OnItemCountChanged?.Invoke(type, GetItemCount(type));
                }
            };
            Managers.Data.OnLoaded += _onDataLoadedHandler;

            // 2) 초기 상태 발행
            if (_itemDatabase != null)
            {
                foreach (var type in _itemDatabase.GetAllItemDefinitions().Keys)
                    OnItemCountChanged?.Invoke(type, GetItemCount(type));
            }
        }

        // =====================
        // ItemDatabase 초기화
        // =====================
        void InitializeItemDatabase()
        {
            try
            {
                _itemDatabase = new ItemDatabase();
                _itemDatabase.Init();
                Debug.Log("ItemManager: ItemDatabase 초기화 완료");
            }
            catch (Exception e)
            {
                Debug.LogError($"ItemManager: ItemDatabase 초기화 실패: {e.Message}");
                // 실패 시 기본 빈 데이터베이스 생성
                _itemDatabase = new ItemDatabase();
            }
        }

        // =====================
        // IItemUse 로직 로드
        // =====================
        void LoadItemUses()
        {
            _itemUseDict = new Dictionary<ItemType, IItemUse>();
            
            try
            {
                var sos = Managers.Resource.LoadAll<ScriptableObject>("Game/ItemUse");
                foreach (var so in sos)
                {
                    if (so is IItemUse logic)
                    {
                        var name = so.name.Replace("Use", "");
                        if (Enum.TryParse<ItemType>(name, out var type)
                            && !_itemUseDict.ContainsKey(type))
                        {
                            _itemUseDict.Add(type, logic);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"ItemManager: ItemUse 로직 로드 실패: {e.Message}");
            }
        }

        // =====================
        // 공개 API
        // =====================

        // 현재 아이템 보유 개수 조회
        public int GetItemCount(ItemType itemId)
        {
            if (Managers.Data.Current == null)
                return 0;
            return GetItemCountInternal(itemId);
        }

        // 아이템 획득
        public void AddItem(ItemType itemId, int amount = 1)
        {
            int newCount = GetItemCount(itemId) + amount;
            SetItemCountInternal(itemId, newCount);
            
            // 데이터 변경 시 즉시 저장
            Managers.Data.SaveDataOnChange();
            
            // 데이터 변경 알림
            Managers.Data.NotifyDataChanged("items", Managers.Data.Current.items);
            
            OnItemCountChanged?.Invoke(itemId, newCount);

            // 서버 호환 아이템 리스트 델타 생성
            GenerateItemsDelta();
            
            // 업적 시스템에 아이템 구매 알림 (첫 획득 시에만)
            //if (GetItemCount(itemId) == amount) // 처음 획득한 경우
            //{
            //    AchievementIntegration.OnItemPurchased(itemId.ToString());
            //}
        }

        // 아이템 사용 시도 (Just_Climb 스타일)
        public bool UseItem(ItemType itemId, GameObject user)
        {
            var data = _itemDatabase?.Get(itemId);
            if (data == null)
                return false;

            // 쿨다운 체크
            if (_nextAvailableTime.TryGetValue(itemId, out var ready)
                && Time.time < ready)
                return false;

            // 소지 & 사용 로직
            if (GetItemCount(itemId) <= 0
                || !_itemUseDict.TryGetValue(itemId, out var logic))
                return false;

            logic.Use(user);  // LampUse, WingUse, FeatherUse, FlagUse 등 

            // 개수 차감 & 이벤트
            RemoveItem(itemId, 1);

            // 버프 적용 기록
            if (data.buffDuration > 0f)
                _buffEndTimes[itemId] = Time.time + data.buffDuration;

            // 쿨다운 기록
            if (data.cooldownDuration > 0f)
                _nextAvailableTime[itemId] = Time.time + data.cooldownDuration;

            // 업적 시스템에 아이템 사용 알림
            // AchievementIntegration.OnItemUsed(itemId.ToString());

            return true;
        }

        /// <summary>
        /// 아이템 사용 가능 여부 확인
        /// </summary>
        /// <param name="itemId">확인할 아이템</param>
        /// <returns>사용 가능하면 true</returns>
        public bool CanUseItem(ItemType itemId)
        {
            bool hasItem = GetItemCount(itemId) > 0;
            bool noCooldown = GetCooldownRemaining(itemId) <= 0;
            return hasItem && noCooldown;
        }

        // 아이템 제거
        public void RemoveItem(ItemType itemId, int amount = 1)
        {
            int newCount = Mathf.Max(0, GetItemCount(itemId) - amount);
            SetItemCountInternal(itemId, newCount);
            
            // 데이터 변경 시 즉시 저장
            Managers.Data.SaveDataOnChange();
            
            // 데이터 변경 알림
            Managers.Data.NotifyDataChanged("items", Managers.Data.Current.items);
            
            OnItemCountChanged?.Invoke(itemId, newCount);

            // 서버 호환 아이템 리스트 델타 생성
            GenerateItemsDelta();
        }

        // 버프 남은 시간 조회
        public float GetBuffRemaining(ItemType itemId)
        {
            return _buffEndTimes.TryGetValue(itemId, out var end)
               ? Mathf.Max(0f, end - Time.time) : 0f;
        }

        // 버프 총 지속 시간 조회
        public float GetBuffDuration(ItemType itemId)
        {
            var data = _itemDatabase?.Get(itemId);
            return data?.buffDuration ?? 0f;
        }

        // 쿨다운 남은 시간 조회
        public float GetCooldownRemaining(ItemType itemId)
        {
            return _nextAvailableTime.TryGetValue(itemId, out var cd)
               ? Mathf.Max(0f, cd - Time.time) : 0f;
        }

        // 총 쿨다운 길이 조회
        public float GetCooldownDuration(ItemType itemId)
        {
            var data = _itemDatabase?.Get(itemId);
            return data?.cooldownDuration ?? 0f;
        }

        // =====================
        // 내부 저장·로드 헬퍼
        // =====================

        /// <summary>
        /// DataManager.Current.items에서 아이템 개수 조회
        /// </summary>
        int GetItemCountInternal(ItemType itemId)
        {
            // DataManager.Current가 null이면 빈 리스트 취급
            var saveData = Managers.Data.Current;
            if (saveData?.items == null)
                return 0;

            // InventoryItem에서 직접 비교
            var item = saveData.items.Find(x => x.itemId == itemId);
            return item?.count ?? 0;
        }

        /// <summary>
        /// DataManager.Current.items 리스트에 아이템 수량 쓰기
        /// </summary>
        void SetItemCountInternal(ItemType itemId, int count)
        {
            var saveData = Managers.Data.Current;
            var list = saveData.items;              // List<InventoryItem> 사용
            
            int idx = list.FindIndex(x => x.itemId == itemId);

            if (idx < 0)
            {
                // 새로 추가
                if (count > 0)
                    list.Add(new InventoryItem(itemId, count));
            }
            else
            {
                if (count > 0)
                    list[idx].count = count;                   // 기존 개수 업데이트
                else
                    list.RemoveAt(idx);                        // 수량 0 → 제거
            }
        }
        /// <summary>
        /// 서버 호환 아이템 델타 생성
        /// </summary>
        private void GenerateItemsDelta()
        {
            var items = Managers.Data.Current.items; // List<InventoryItem>
            Managers.Data.NotifyDataChanged("items", items);
        }

        /// <summary>
        /// ItemManager 정리 (Managers에서 호출)
        /// </summary>
        public void Clear()
        {
            Debug.Log("ItemManager: Clear");
            
            // 이벤트 해제
            if (Managers.Data != null && _onDataLoadedHandler != null)
                Managers.Data.OnLoaded -= _onDataLoadedHandler;

            // 외부 구독자들이 남아있을 수 있으니 이벤트 자체를 초기화
            OnItemCountChanged = null;
            
            // 딕셔너리 정리
            _itemUseDict?.Clear();
            _buffEndTimes?.Clear();
            _nextAvailableTime?.Clear();
            
            // 참조 해제
            _itemDatabase = null;
            _onDataLoadedHandler = null;
        }
    }
