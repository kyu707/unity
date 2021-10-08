using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//총알이 세발 맞았을때 폭발하도록 하는 script 생성
public class BarrelCtrl : MonoBehaviour {
    //폭발효과 프리팹을 저장할 변수
    public GameObject expEffect;
    //찌그러진 드럼통의 메쉬를 저장할 배열
    public Mesh[] meshes; // 현재는 배열의 요소가 몇개인지 모름->Unity에서 설정
    //드럼통의 텍스쳐를 저장할 배열
    public Texture[] textures;

    //총알이 맞은 횟수
    private int hitCount = 0;
    //Rigidbody 컴포넌트 저장변수
    private Rigidbody rb;
    //MeshFilter 컴포넌트 변수
    private MeshFilter meshFilter;
    //MeshRenderer 컴포넌트를 저장할 변수
    private MeshRenderer _renderer;
    //AudioSource 컴포넌트를 저장할 변수
    private AudioSource _audio;

    //폭발 반경 변수
    public float expRadius = 10.0f;
    //폭발음 오디오 클립
    public AudioClip expSfx;

    //Shake 클래스를 저장할 변수
    public Shake shake;

    void Start()
    {
        //Rigidbody 컴포넌트 추출해서 저장
        rb = GetComponent<Rigidbody>();
        //MeshFilter 컴포넌트를 추출해서 저장
        meshFilter = GetComponent<MeshFilter>();

        //MeshRenderer  컴포넌트를 추출해 저장
        _renderer = GetComponent<MeshRenderer>();
        //AudioSource 컴포넌트를 추출해 저장
        _audio = GetComponent<AudioSource>();

        //Shake 스크립트를 추출
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();


        //난수를 발생 시켜 불규칙한 텍스쳐를 적용
        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
    }

    //충돌이 발생했을때 한번 호출되는 콜백함수
    void OnCollisionEnter(Collision coll)
    {
        //충돌한 게임 오브젝트의 태그를 비교
        if(coll.collider.CompareTag("BULLET"))
        {
            //총알의 충돌 횟수를 증가시키고 3발 이상 맞았는지 확인
            if(++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }
    void ExpBarrel()
    {
        //폭발효과 공장(프리팹)을 동적으로 생성
        //              (프리팹의 변수, 생성위치, 각도)
       GameObject effect= Instantiate(expEffect, transform.position, Quaternion.identity);
        // Quaternion.identity: Quaternion을 써야하는데 rotation정보는 필요 없을때 쓴다                                                              
        Destroy(effect, 2.0f); // (삭제할 객체, 지연시간)
        //Rigidbody 컴포넌트의 mass를 1.0으로 수정해서 가볍게 만든다.
        //rb.mass = 1.0f;
        //위로 솟구치는 힘을 가함
        //rb.AddForce(Vector3.up * 1000.0f);

        //폭발력 생성
        IndirectDamage(transform.position);

        //난수를 발생
        int idx = Random.Range(0, meshes.Length);
        //찌그러진 메쉬를 적용
        meshFilter.sharedMesh = meshes[idx];

        //폭발음 발생
        _audio.PlayOneShot(expSfx, 0.5f);

        //셰이크 효과 호출
        StartCoroutine(shake.ShakeCamera(0.1f, 0.2f, 0.5f));
    }

    //폭발력을 주위에 전달하는 함수
    void IndirectDamage(Vector3 pos)
    {
        //주변에 있는 드럼통을 전부 추출(Physics.OverlapSphere)
        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << 8);

        foreach(var coll in colls) // 드럼통을 하나하나 순회(배열)
        {
            //폭발범위에 포함된 드럼통의 rigidbody 컴포넌트를 추출
            var _rb = coll.GetComponent<Rigidbody>();
            //드럼통의 무게를 가볍게!
            _rb.mass = 1.0f;
            //폭발력을 전달
            _rb.AddExplosionForce(1200.0f, pos, expRadius, 1000.0f);
            //                              (옆폭발력, 원점(폭발한 barrel의 원점), 반경, 위로 솟구치는 힘)
        }
    }
}
