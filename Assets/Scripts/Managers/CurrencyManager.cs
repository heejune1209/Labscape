using System;
using Labscape.Data;
using Labscape.Manager;
using UnityEngine;

namespace Labscape.Manager
{
    /// <summary>
    /// 재화(골드,보석) 관리 전담.
    /// DataManager의 이벤트를 구독하고, UI나 다른 로직에 OnSpannerChanged/OnCoreChanged만 노출.
    /// </summary>
    public class CurrencyManager : IDisposable
    {
        public event Action<int> OnSpannerChanged;
        public event Action<int> OnCoreChanged;

        /// <summary>현재 골드</summary>
        public int Spanner { get; private set; }
        /// <summary>현재 보석</summary>
        public int Core { get; private set; }

        // 싱글톤을 통해 DataManager에 접근

        /// <summary>
        /// CurrencyManager 초기화
        /// </summary>
        public void Init()
        {
            Debug.Log("CurrencyManager: Initialize");
            
            // 데이터 로드 완료 시 값 갱신
            Managers.Data.OnLoaded += UpdateCurrencies;

            // Current가 세팅된 경우에만 최초 갱신 (DataManager.Init()에서 이미 데이터 로드됨)
            if (Managers.Data.Current != null)
                UpdateCurrencies(Managers.Data.Current);
        }

        /// <summary>
        /// DataManager.Current를 보고 _spanner/core 갱신 및 이벤트 발행
        /// </summary>
        void UpdateCurrencies(SaveData save)
        {
            Spanner = save.spanner;
            OnSpannerChanged?.Invoke(Spanner);

            Core = save.core;
            OnCoreChanged?.Invoke(Core);
        }

        // 외부 API
        public int GetSpanner() { return Spanner; }
        public int GetCore() { return Core; }

        // 골드 추가. DataManager.Current.spanner 변경 후 Save().
        public void AddSpanner(int amount)
        {
            Managers.Data.Current.spanner += amount;
            
            // 데이터 변경 시 즉시 저장
            Managers.Data.SaveDataOnChange();
            
            // 데이터 변경 알림
            Managers.Data.NotifyDataChanged("spanner", Managers.Data.Current.spanner);
            
            // 로컬 값 업데이트 및 이벤트 발생
            Spanner = Managers.Data.Current.spanner;
            OnSpannerChanged?.Invoke(Spanner);
        }

        // 골드 사용. 충분히 있으면 차감 후 Save() 하고 true, 아니면 false.
        public bool SpendSpanner(int amount)
        {
            if (Spanner < amount)
                return false;

            Managers.Data.Current.spanner -= amount;
            
            // 데이터 변경 시 즉시 저장
            Managers.Data.SaveDataOnChange();
            
            // 데이터 변경 알림
            Managers.Data.NotifyDataChanged("spanner", Managers.Data.Current.spanner);
            
            // 로컬 값 업데이트 및 이벤트 발생
            Spanner = Managers.Data.Current.spanner;
            OnSpannerChanged?.Invoke(Spanner);
            
            return true;
        }

        // 젬 추가. DataManager.Current.core 변경 후 Save().
        public void AddCore(int amount)
        {
            Managers.Data.Current.core += amount;
            
            // 데이터 변경 시 즉시 저장
            Managers.Data.SaveDataOnChange();
            
            // 데이터 변경 알림
            Managers.Data.NotifyDataChanged("core", Managers.Data.Current.core);
            
            // 로컬 값 업데이트 및 이벤트 발생
            Core = Managers.Data.Current.core;
            OnCoreChanged?.Invoke(Core);
        }

        // 젬 사용. 충분히 있으면 차감 후 Save() 하고 true, 아니면 false.
        public bool SpendCore(int amount)
        {
            if (Core < amount)
                return false;

            Managers.Data.Current.core -= amount;
            
            // 데이터 변경 시 즉시 저장
            Managers.Data.SaveDataOnChange();
            
            // 데이터 변경 알림
            Managers.Data.NotifyDataChanged("core", Managers.Data.Current.core);
            
            // 로컬 값 업데이트 및 이벤트 발생
            Core = Managers.Data.Current.core;
            OnCoreChanged?.Invoke(Core);
            
            return true;
        }

        // 메모리 누수 방지
        public void Dispose()
        {
            Managers.Data.OnLoaded -= UpdateCurrencies;
        }
        
        /// <summary>
        /// CurrencyManager 정리 (Managers에서 호출)
        /// </summary>
        public void Clear()
        {
            Debug.Log("CurrencyManager: Clear");
            
            // 이벤트 정리
            Dispose();

            // 이벤트 초기화
            OnSpannerChanged = null;
            OnCoreChanged = null;

            // 값 초기화
            Spanner = 0;
            Core = 0;
        }
    }
}
