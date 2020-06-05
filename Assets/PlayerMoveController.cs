using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMoveController : MonoBehaviourPunCallbacks, IPunObservable
{

    #region Public Field

    public Animator AN;
    public PhotonView PV;
    public CharacterController CC;
    public Text NickNameText;
    public Image HealthImage;

    #endregion

    #region Private Field

    private bool Attacking;

    #endregion

    #region MonoBehaviourCallback

    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
        Attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            Move();
            if (!Attacking)
            {
                Mouse2Act();
            }
        }
        else
        {
            PhotonMove();
        }
    }
    #endregion

    #region PlayerMove method

    // 이동 관련 변수

    //중력값
    const float GravityPower = 9.8f;
    //목적지에 도착했다고 보는 정지 거리.
    const float StoppingDistance = 0.6f;
    //도착했는가
    public bool arrived = false;
    //방향을 강제로 지시하는가
    bool forceRotate = false;
    //강제로 향하게 하고 싶은 방향
    Vector3 forceRotateDirection;
    //목적지
    public Vector3 destination;
    //회전속도
    public float rotationSpeed = 360.0f;
    //이동 속도
    public float walkSpeed = 6.0f;
    //현재 이동 속도
    Vector3 velocity = Vector3.zero; //0,0,0



    public void Move()
    {
        //이동 속도 velocity를 갱신한다.
        if (CC.isGrounded)
        {
            
            //수평면에서 이동을 고려하므로 XZ만 다룬다.
            Vector3 destinationXZ = destination;
            //목적지와 현재 위치 높이를 똑같이 한다.
            destinationXZ.y = transform.position.y;

            //목적지까지 거리와 방향을 구합니다.
            Vector3 direction = (destinationXZ - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, destinationXZ);

            //현재 속도를 보관한다.
            Vector3 currentVelocity = velocity;

            //목적지에 가까이 왔으면 도착.
            if (arrived || distance < StoppingDistance)
                arrived = true;
            //이동 속도를 구한다.
            if (arrived)
                velocity = Vector3.zero;
            else
                velocity = direction * walkSpeed;
            //부드럽게 보간 처리
            velocity = Vector3.Lerp(currentVelocity, velocity,
                Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            MoveAni();
            PV.RPC("PunMoveAni", RpcTarget.AllBuffered, velocity);

            velocity.y = 0;
            if (!forceRotate)
            {
                //바꾸고 싶은 방향으로 변경한다.
                if (velocity.magnitude > 0.1f && !arrived)
                {
                    //이동하지 않았다면 방향은 변경하지 않는다.
                    Quaternion characterTargetRotation =
                        Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                    characterTargetRotation, rotationSpeed * Time.deltaTime);

                }

            }
            else
            {
                //강제로 방향을 지정한다.
                Quaternion characterTargetRotation =
                       Quaternion.LookRotation(forceRotateDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                characterTargetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        
        //중력
        velocity += Vector3.down * GravityPower * Time.deltaTime;

        //땅에 닿아 있다면 지면을 눌러준다.
        Vector3 snapGround = Vector3.zero;
        if (CC.isGrounded)
            snapGround = Vector3.down;

        //캐릭터 컨트롤러를 사용해서 움직인다.
        CC.Move(velocity * Time.deltaTime + snapGround);

        //강제로 방향 변경을 해제한다.
        if (forceRotate && Vector3.Dot(transform.forward, forceRotateDirection) > 0.99f)
            forceRotate = false;
    }

    //목적지를 설정한다. 인수 destination은 목적지
    public void SetDestination(Vector3 destination)
    {
        arrived = false;
        this.destination = destination;
    }

    //지정한 방향으로 향한다.
    public void SetDirection(Vector3 direction)
    {
        forceRotateDirection = direction;
        forceRotateDirection.y = 0;
        forceRotateDirection.Normalize();
        forceRotate = true;
    }


    //이동을 그만둔다.
    public void StopMove()
    {
        //현재 지점을 목적지로 한다.
        destination = transform.position;
    }

    //목적지에 도착했는지 조사한다.
    public bool Arrived()
    {
        return arrived;
    }



    #endregion

    #region PlayerControll method


    public float attackRange = 5.5f;
    //플레이어 컨트롤 관련 변수
    const float RayCastMaxDistance = 1000.0f;
    Vector3 attackTarget;

    void Mouse2Act()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("클릭");
            // RayCast로 대상물을 조사한다.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, RayCastMaxDistance, (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("EnemyHit"))))
            {
                // 지면이 클릭되었다.
                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    Debug.Log("지면클릭");
                SetDestination(hitInfo.point);
                // 적이 클릭되었다.
                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("EnemyHit"))
                {
                    // 수평 거리를 체크해서 공격할지 결정한다.
                    Debug.Log("적군클릭");
                    Vector3 hitPoint = hitInfo.point;
                    hitPoint.y = transform.position.y;
                    float distance = Vector3.Distance(hitPoint, transform.position);
                    if (distance < attackRange)
                    {
                        // 공격.
                        attackTarget = hitInfo.collider.transform.position;
                        AttackStart(attackTarget);
                        PV.RPC("PunAttackStart", RpcTarget.AllBuffered,attackTarget);
                    }
                    else
                    SetDestination(hitInfo.point);
                }
            }
        }
    }

    public void StartAttack()
    {
        Attacking = true;
    }

    public void EndAttack()
    {
        Attacking = false;
    }

    #endregion

    #region Animator Method


    //애니메이션구현 함수

    public void MoveAni()
    {
        AN.SetFloat("Speed", velocity.magnitude / Time.deltaTime);
    }

    public void AttackStart(Vector3 target)
    {
        AN.SetTrigger("Attacking");
        //StartAttack();
        //적 방향을 바라봄
        Vector3 targetDirection = (target - transform.position).normalized;
        SetDirection(targetDirection);
        //이동을 멈춤
        StopMove();
    }
    public void Skill1Start()
    {
        AN.SetTrigger("Skill1");
    }
    public void Skill2Start()
    {
        AN.SetTrigger("Skill2");
    }
    public void Skill3Start()
    {
        AN.SetTrigger("Skill3");
    }
    public void SkillRStart()
    {
        AN.SetTrigger("SkillR");
    }

    #endregion

    #region PunCallback


    //포톤 변수
    private Vector3 curPos;
    private Quaternion curRot;
    private bool curAtk;
    private Vector3 curtarget;
    public void PhotonMove()
    {
        transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10.0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, curRot, Time.deltaTime * 10.0f);
        MoveAni();
    }

    [PunRPC]
    void PunMoveAni(Vector3 _velocity) => velocity = _velocity;

    [PunRPC]
    void PunAttackStart(Vector3 _tartget)
    {
        AN.SetTrigger("Attacking");
        curtarget = _tartget;
        //적 방향을 바라봄
        Vector3 targetDirection = (curtarget - transform.position).normalized;
        SetDirection(targetDirection);
        //이동을 멈춤
        StopMove();
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(HealthImage.fillAmount);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
            HealthImage.fillAmount = (float)stream.ReceiveNext();
        }
    }
    #endregion


}
