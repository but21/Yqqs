using UnityEngine;

namespace Script
{
    public class PlayerController : MonoBehaviour
    {
        // 控制角色移动向量
        private Vector3 newPosition;

        // 角色移动速度
        public float moveSpeed = 5;

        // 角色刚体组件
        private Rigidbody _rigidbody;

        // 角色控制器
        private CharacterController _cc;

        // 角色模型
        private Transform _model;

        // 射线, 用于与地面碰撞, 以控制角色朝向
        private Ray _ray;

        // 射线获取到的信息
        private RaycastHit _hitInfo;

        private Vector3 _moveDir;


        public Animator animator;

        public bool isSimple;
        public MoveType moveType;

        public enum MoveType
        {
            刚体,
            角色控制器,
            更新坐标
        }

        void Start()
        {
            _model = transform.GetChild(0);
            animator = _model.GetComponent<Animator>();

            switch (moveType)
            {
                case MoveType.刚体:
                    _rigidbody = GetComponent<Rigidbody>();
                    if (_rigidbody == null)
                    {
                        _rigidbody = gameObject.AddComponent<Rigidbody>();
                    }

                    _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    break;
                case MoveType.角色控制器:
                    _cc = GetComponent<CharacterController>();
                    if (_cc == null)
                    {
                        _cc = gameObject.AddComponent<CharacterController>();
                    }

                    _cc.height = 2.04f;
                    _cc.center = new Vector3(0, 1, 0);
                    _cc.enabled = true;
                    break;
            }
        }

        void Update()
        {
            BeforeMove();
            Movement();
            Rotate();
            atk();
        }


        void BeforeMove()
        {
            // 角色移动方向
            _moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxis("Vertical"));

            newPosition = _moveDir * moveSpeed * Time.deltaTime;
        }

        void Movement()
        {
            // 移动方法一
            // 角色位置的改变(移动)
            // transform.position += newPosition;

            // 移动方法二
            // 利用刚体组件
            // _rigidbody.velocity = newPosition;

            // 移动方法三
            // 利用CharacterController组件
            // _cc.Move(newPosition);

            // 角色移动动画
            animator.SetBool("移动", _moveDir != Vector3.zero);
            switch (moveType)
            {
                case MoveType.刚体:
                    _rigidbody.velocity = newPosition * 100;
                    break;
                case MoveType.角色控制器:
                    if (isSimple)
                    {
                        _cc.SimpleMove(newPosition * 100);
                    }
                    else
                    {
                        _cc.Move(newPosition);
                    }

                    break;
                case MoveType.更新坐标:
                    transform.position += newPosition;
                    break;
            }
        }

        // 控制角色转向
        void Rotate()
        {
            // 定义从屏幕中鼠标位置发射的射线
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 发射射线
            if (Physics.Raycast(_ray, out _hitInfo, 1000, 1 << 6))
            {
                // 获取射线击中点的位置信息
                Vector3 target = _hitInfo.point;
                // 使目标点高度和角色一样高, 使角色只绕y轴旋转, 不会倾斜
                target.y = _model.position.y;
                // 使模型朝向目标点
                _model.LookAt(target);
            }
        }

        void atk()
        {
            // 按下鼠标左键
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("射击ing", true);
            }

            if (Input.GetMouseButtonUp(0))
            {
                animator.SetBool("射击ing", false);
            }
        }
    }
}