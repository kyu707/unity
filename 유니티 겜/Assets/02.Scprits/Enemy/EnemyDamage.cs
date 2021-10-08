using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
	private const string bulletTag = "BULLET";
	//생명 게이지
	private float hp = 100.0f;
	//피격시 사용할 혈흔 효과
	private GameObject bloodEffect;

	// Use this for initialization
	void Start()
	{
		//혈흔효과 프리팹을 로드
        //유니티에 만들어 놓은(예약된) 폴더를 가져올 때 아래와 같은 코드를 사용(중요!2)
		bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
	}

	// Update is called once per frame
	void OnCollisionEnter(Collision coll)
	{
		if (coll.collider.tag == bulletTag)
		{
			//혈흔 효과를 생성하는 함수 호출
			ShowBloodEffect(coll);
			//총알 삭제
			Destroy(coll.gameObject);
			//생명 게이지 차감
            //다른 스크립트의 변수를 가져올때 사용하는 방식(중요1)
			hp -= coll.gameObject.GetComponent<BulletCtrl>().damage;
			if (hp <= 0.0f)
			{
				//적 캐릭터 상태를 DIE로 변경
				GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
			}
		}
	}
	//혈흔 효과를 생성하는 함수
	void ShowBloodEffect(Collision coll)
	{
		//총알이 충돌한 지점 산출
		Vector3 pos = coll.contacts[0].point;
		//총알이 충돌했을 때의 법선 벡터
		Vector3 _normal = coll.contacts[0].normal;
		//총알의 충돌 시 방향 벡터의 회전값 계산
		Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);

		//혈흔 효과 생성
		GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot);
		Destroy(blood, 1.0f);
	}
}

