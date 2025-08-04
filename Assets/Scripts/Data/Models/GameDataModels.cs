using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Labscape.Data
{
    /// <summary>
    /// Unity Vector3 직렬화용 구조체 (Labscape 클라이언트 전용)
    /// 깃발 위치, 스폰 포인트 등에 사용
    /// </summary>
    [Serializable]
    public struct SerializableVector3
    {
        [JsonProperty("x")]
        public float x;
        
        [JsonProperty("y")]
        public float y;
        
        [JsonProperty("z")]
        public float z;
        
        public SerializableVector3(float x, float y, float z)
        {
            this.x = x; 
            this.y = y; 
            this.z = z;
        }
        
        public SerializableVector3(Vector3 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }
        
        public Vector3 ToVector3() => new Vector3(x, y, z);
        
        public static implicit operator Vector3(SerializableVector3 sv) => sv.ToVector3();
        public static implicit operator SerializableVector3(Vector3 v) => new SerializableVector3(v);
        
        public override string ToString()
        {
            return $"({x:F2}, {y:F2}, {z:F2})";
        }
    }
}

namespace Labscape.Items
{
    /// <summary>
    /// 클라이언트 전용 아이템 모델 (Labscape 게임용)
    /// 서버 없이 로컬 JSON 저장/로드만 처리
    /// </summary>
    [Serializable]
    public class InventoryItem
    {
        // 아이템 고유 ID (enum을 문자열로 자동 변환)
        [JsonProperty("itemId")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ItemType itemId;

        // 보유 개수
        [JsonProperty("count")]
        public int count;

        public InventoryItem() { }

        public InventoryItem(ItemType itemId, int count)
        {
            this.itemId = itemId;
            this.count = count;
        }

        public override string ToString()
        {
            return $"{itemId}: {count}";
        }
    }
}
    
namespace Labscape.Data
{
    /// <summary>
    /// 메인 게임 데이터 저장 클래스 (Labscape 클라이언트 전용)
    /// 로컬 JSON 파일로만 저장/로드
    /// </summary>
    [Serializable]
    public class SaveData
    {
        // 골드 개수
        [JsonProperty("spanner")]
        public int spanner = 0;

        // 보석 개수
        [JsonProperty("core")]
        public int core = 0;

        // 현재 선택된 캐릭터의 이름
        [JsonProperty("selectedCharacter")]
        public string selectedCharacter = "Default";

        // 보유 중인 아이템 리스트
        [JsonProperty("items")]
        public List<Labscape.Items.InventoryItem> items = new List<Labscape.Items.InventoryItem>();

        // 스테이지별 클리어 여부 (true = 클리어)
        [JsonProperty("stageClears")]
        public List<bool> stageClears = new List<bool>();

        // 스테이지별 깃발(체크포인트) 위치 저장
        [JsonProperty("stageFlagPositions")]
        public List<SerializableVector3> stageFlagPositions = new List<SerializableVector3>();

        // 스테이지별 최고 보상(획득 보석 개수)
        [JsonProperty("bestCoreRewards")]
        public List<int> bestCoreRewards = new List<int>();

        // 스테이지별 최단 클리어 타임(초) - 개인 기록용
        [JsonProperty("bestClearTimes")]
        public List<float> bestClearTimes = new List<float>();

        // 스테이지별 최소 사망 횟수 - 개인 기록용
        [JsonProperty("bestDeathCounts")]
        public List<int> bestDeathCounts = new List<int>();

        // 유지되는 필드들 (플레이 중 임시 저장용)
        // 중간 저장된 진행 시간(일시정지 등)
        [JsonProperty("currentPlayTimes")]
        public List<float> currentPlayTimes = new List<float>();
        
        // 중간 저장된 사망 횟수(일시정지 등)
        [JsonProperty("currentDeathCounts")]
        public List<int> currentDeathCounts = new List<int>();

        // 데이터 구조 버전 (필요 시 호환성 체크용)
        [JsonProperty("version")]
        public int version = 1;

        /// <summary>
        /// 디버깅용 데이터 요약 출력
        /// </summary>
        public override string ToString()
        {
            return $"SaveData[Spanner:{spanner}, Core:{core}, Items:{items.Count}, Character:{selectedCharacter}]";
        }
    }
}