using System;
using UnityEngine;
using UnityEngine.InputSystem;
using ChickenOut.Battle.Items;
using ChickenOut.Battle.Managers;

namespace ChickenOut.Battle.Chickens
{
    public class ChickenMovement : MonoBehaviour
    {
        #region Stuff
        protected const string SPEED = "Speed", IS_JUMPING = "IsJumping", IS_LANDING = "IsLanding", IS_DASHING = "IsDashing";

        #region Events
        public Action onJumped;
        public Action onLanded;
        public Action<int, float> onDashed;

        public Action<bool> CallRPCFlipping;
        public Action<bool> onFlipped;
        #endregion

        #region Inputs
        protected InputAction _moveAction;
        protected InputAction _dashAction;
        #endregion

        private ChickenInfo _myInfo;

        [HideInInspector] public Rigidbody2D rigidBody;
        protected SoundController _soundController;
        protected Animator _animator;

        protected float _speed = 5f;
        protected bool _isArrows;

        #region Jump
        protected float _jumpStrength = 10f;
        protected int _maxJumps = 1;
        protected bool _hasJumped;
        protected int _timesJumped;

        [HideInInspector] internal bool isGrounded;
        #endregion

        #region Dash
        protected float _dashForce = 20f;

        protected float _startDashTimer = .35f;
        protected float _dashChargeTime = 2f;
        protected float _currentdashtime;

        protected int _dashDirection;
        protected bool _hasDashed;
        protected bool _canDash = true;
        #endregion

        #endregion

        #region Starting
        protected virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            rigidBody = GetComponent<Rigidbody2D>();

            GroundCheck groundCheck = GetComponentInChildren<GroundCheck>();
            if (groundCheck != null)
                groundCheck.onGrounded += CheckGround;

            ItemPicker itemPicker = GetComponent<ItemPicker>();
            if (itemPicker.enabled)
            {
                itemPicker.onExtraJumped += ExtraJump;
                itemPicker.onSpeedBoosted += SpeedBoost;
            }
        }
        protected virtual void Start() => _soundController = SoundController.instance;

        public void DoControls(PlayerInput input)
        {
            _moveAction = input.actions["Move"];
            _dashAction = input.actions["Dash"];
        }
        public void GetInfo(ChickenInfo info) => _myInfo = info;
        #endregion

        #region Updates
        void Update() => UpdateFrame();
        void FixedUpdate() => FixedUpdateFrame();
        protected void UpdateFrame(bool isMine = true)
        {
            if (IsInputNull || !isMine)
                return;

            Vector2 movement = _moveAction.ReadValue<Vector2>();

            float dasher = _isArrows ? _dashAction.ReadValue<float>() : 0;

            if (_canDash && !_hasDashed && ((_isArrows && dasher != 0) || (!_isArrows && Mathf.Abs(movement.x) > .95f)))
                ShouldDash(movement, (int)dasher);

            #region Checking
            Animation(movement);

            if (movement.y < .5f)
                _hasJumped = false;

            if (movement.x < .3f)
                _hasDashed = false;
            #endregion
        }
        protected void FixedUpdateFrame(bool isMine = true)
        {
            if (IsInputNull || !isMine)
                return;

            Vector2 movement = _moveAction.ReadValue<Vector2>();

            bool yCheck = (!_isArrows && movement.y >= .9f) || (_isArrows && movement.y > 0f);
            bool canJump = _timesJumped < _maxJumps && rigidBody.velocity.y <= 0 && !_hasJumped;
            if (canJump && yCheck)
                Jump();

            if (!IsDashing)
                Move(movement);

            if (IsDashing)
                Dash();
        }
        bool IsInputNull => _moveAction == null || _dashAction == null;
        #endregion

        #region Visually
        protected void CheckFlipping(float x)
        {
            if (x == 0 || _animator.transform.localScale.x == 0)
                return;

            if (IsGoingRight(x) && !IsFacingRight)
                Flipping(true);
            else if (!IsGoingRight(x) && IsFacingRight)
                Flipping(false);
        }
        void Flipping(bool flip)
        {
            Flip(flip);
            CallRPCFlipping?.Invoke(flip);
        }
        protected void Flip(bool flip) => onFlipped?.Invoke(flip);

        protected void Animation(Vector2 movement)
        {
            if (isGrounded)
            {
                _animator.SetFloat(SPEED, Mathf.Abs(movement.normalized.x));
                SetBools(false, false);
            }
            else
                SetBools(rigidBody.velocity.y > 0, rigidBody.velocity.y < -1f);
        }
        void SetBools(bool isJumping, bool isLanding)
        {
            _animator.SetBool(IS_JUMPING, isJumping);
            _animator.SetBool(IS_LANDING, isLanding);
        }

        bool IsGoingRight(float x) => x > 0;
        bool IsFacingRight => _animator.transform.localScale.x < 0;
        #endregion

        #region Movement
        public void CheckGround(bool enter)
        {
            if (rigidBody.velocity.y > 0)
                return;

            if (enter && !isGrounded)
            {
                onLanded?.Invoke();
                _timesJumped = 0;
            }

            isGrounded = enter;
        }
        void Move(Vector2 movement)
        {
            rigidBody.velocity = new Vector2(movement.normalized.x * _speed, rigidBody.velocity.y);

            if (movement.x != 0)
                CheckFlipping(movement.x);
        }

        void Jump()
        {
            onJumped?.Invoke();
            JumpingEffects(); // for now
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, _jumpStrength);

            _timesJumped++;
            _hasJumped = true;
            isGrounded = false;
        }
        #endregion

        #region Dash
        void ShouldDash(Vector2 movement, int dasher)
        {
            onDashed?.Invoke(_dashDirection, _startDashTimer);
            DashingEffects(_dashDirection); // for now
            Invoke(nameof(MakeCanDash), _dashChargeTime);

            _canDash = false;
            _hasDashed = true;

            _dashDirection = _isArrows ? dasher : (movement.x < 0 ? -1 : 1);
            CheckFlipping(_dashDirection);

            _currentdashtime = _startDashTimer;
            rigidBody.velocity = Vector2.zero;
        }
        void Dash()
        {
            rigidBody.velocity = transform.right * (_dashForce * _dashDirection);
            _currentdashtime -= Time.deltaTime;
        }
        void MakeCanDash() => _canDash = true;
        bool IsDashing => _currentdashtime > 0;
        #endregion

        #region Pickup

        #region SpeedBoost
        void SpeedBoost(float duration)
        {
            _speed *= 2;
            Invoke(nameof(ResetSpeed), duration);
        }
        void ResetSpeed() => _speed /= 2;
        #endregion

        #region ExtraJump
        void ExtraJump(float duration)
        {
            _maxJumps++;
            Invoke(nameof(RemoveExtraJump), duration);
        }
        void RemoveExtraJump() => _maxJumps--;
        #endregion

        #endregion

        #region Effects
        protected void JumpingEffects()
        {
            _soundController.JumpSound();
        }

        protected void DashingEffects(int direction)
        {
            _soundController.DashSound();
            _animator.SetTrigger(IS_DASHING);
        }
        protected void DashingEffects(int direction, bool something)
        {
            _soundController.DashSound();
            if (something)
                _animator.SetTrigger(IS_DASHING);
        }
        #endregion
    }
}