using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 키를 누르면 애니메이션을 전환한다 
public class OnKeyPress_ChangeAnime : MonoBehaviour
{
    
    public string upAnime = "";     // 위 방향 ：Inspector에 지정
    public string downAnime = "";   // 아래 방향：Inspector에 지정
    public string rightAnime = "";  // 오른쪽 방향：Inspector에 지정
    public string leftAnime = "";   // 왼쪽 방향：Inspector에 지정

    string nowMode = "";
    string oldMode = "";

    void Start()// 처음에 시행한다 
    { 
        nowMode = downAnime;
        oldMode = "";
    }

    void Update()// 계속 시행한다 
    { 
        if (Input.GetKey("up"))// 위 키면
        { 
            nowMode = upAnime;
        }
        if (Input.GetKey("down"))// 아래 키면
        { 
            nowMode = downAnime;
        }
        if (Input.GetKey("right"))// 오른쪽 키면
        { 
            nowMode = rightAnime;
        }
        if (Input.GetKey("left"))// 왼쪽 키면
        { 
            nowMode = leftAnime;
        }
    }
    void FixedUpdate() // 계속 시행한다(일정 시간마다)
    { 
        // 만약 다른 키가 눌리면 
        if (nowMode != oldMode)
        {
            oldMode = nowMode;
            // 애니메이션을 전환한다 
            Animator animator = this.GetComponent<Animator>();
            animator.Play(nowMode);
        }
    }
}
