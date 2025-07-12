using UnityEngine;
using UnityEngine.EventSystems;

namespace ASPax.Handlers
{
    using ASPax.Attributes.Drawer;
    using ASPax.Attributes.Drawer.SpecialCases;
    using ASPax.Attributes.Meta;
    using ASPax.Attributes.Validator;
    using ASPax.Utilities;
    /// <summary>
    /// Easy and safety access of all elements from transform, rectTransform and Screen/World Positions
    /// </summary>
    [ExecuteAlways]
    public class TransformHandler : UIBehaviour
    {
        [Header(Header.MANAGEABLE, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [MinValue(0.001f), SerializeField] private float rangeRatioHandler;

        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [SerializeField, ReadOnly] private bool isRectTransform;
        [SerializeField, ReadOnly, ShowIf(nameof(isRectTransform))] private Rect rect;
        [SerializeField, ReadOnly, ShowIf(nameof(isRectTransform))] private float ratio;
        [SerializeField, ReadOnly, ShowIf(nameof(isRectTransform))] private float result;

        [Header(Header.components, order = 0), HorizontalLine]
        [SerializeField, ReadOnly, ShowIf(nameof(isRectTransform))] private RectTransform rectTransform;
        [SerializeField, ReadOnly, ShowIf(nameof(isRectTransform))] private Camera mainCamera;
#if UNITY_EDITOR
        [Header("VALUES - Editor Test", order = 0), HorizontalLine]
        [SerializeField, ReadOnly] private Vector3 position;
        [SerializeField, ReadOnly] private Vector3 localPosition;
        [SerializeField, ReadOnly] private Vector3 anchoredPosition;
        [SerializeField, ReadOnly] private Vector3 localScale;
        [Space(20, order = 0)]
        [SerializeField, ReadOnly] private Quaternion rotation;
        [SerializeField, ReadOnly] private Quaternion localRotation;
        [SerializeField, ReadOnly] private Vector3 eulerAngles;
        [SerializeField, ReadOnly] private Vector3 localEulerAngles;
        [Space(20, order = 0)]
        [SerializeField, ReadOnly] private Vector3 screenToWorldPointPosition;
        [SerializeField, ReadOnly] private Vector3 screenToWorldPointLocalPosition;
        [SerializeField, ReadOnly, ShowIf(nameof(isRectTransform))] private Vector3 screenToWorldPointAnchoredPosition;
        /// <inheritdoc/>
        [Button(nameof(Reset))]
        protected override void Reset()
        {
            base.Reset();
            mainCamera = Camera.main;
            isRectTransform = TryGetComponent(out rectTransform);
            rangeRatioHandler = 1f;
            UpdateValues();
            Discard();
        }
        /// <inheritdoc/>
        protected override void OnValidate()
        {
            base.OnValidate();
            UnityEditor.EditorApplication.delayCall += () =>
            {
                var conditional =
                UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode ||
                UnityEditor.EditorApplication.isCompiling ||
                UnityEditor.EditorApplication.isUpdating;

                if (conditional) return;

                try
                {
                    if (mainCamera == null) mainCamera = Camera.main;

                    if (rectTransform == null) isRectTransform = TryGetComponent(out rectTransform);
                    else isRectTransform = true;

                    UpdateValues();
                }
                catch
                {
                    return;
                }
            };
        }
        /// <summary>
        /// Assign values to the serialized fields
        /// </summary>
        private void AssignValuesToInspector()
        {
            position = Position;
            localPosition = LocalPosition;
            rotation = Rotation;
            localRotation = LocalRotation;
            eulerAngles = EulerAngles;
            localEulerAngles = LocalEulerAngles;
            localScale = Scale;
            anchoredPosition = GetAnchoredPosition();
            screenToWorldPointPosition = ScreenToWorldPointPosition;
            screenToWorldPointLocalPosition = ScreenToWorldPointLocalPosition;
            screenToWorldPointAnchoredPosition = ScreenToWorldPointAnchoredPosition;
        }
        /// <summary>
        /// Discard variables used only for display in the Inspector
        /// </summary>
        private void Discard()
        {
            _ = position;
            _ = localPosition;
            _ = anchoredPosition;
            _ = localScale;
            _ = rotation;
            _ = localRotation;
            _ = eulerAngles;
            _ = localEulerAngles;
            _ = screenToWorldPointPosition;
            _ = screenToWorldPointLocalPosition;
            _ = screenToWorldPointAnchoredPosition;
        }
        /// <inheritdoc/>
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            UpdateValues();
        }
#endif
        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();
            ComponentsAssignment();
        }
        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();
            mainCamera = Camera.main;
            UpdateValues();
        }
        /// <summary>
        /// Assignment of components and variables
        /// </summary>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        private void ComponentsAssignment()
        {
            if (rectTransform == null)
            {
                isRectTransform = TryGetComponent(out RectTransform rectTransform);
                if (isRectTransform) this.rectTransform = rectTransform;
                else this.rectTransform = null;
            }
            else
            {
                isRectTransform = true;
            }
        }
        /// <summary>
        /// Set the anchored position of the RectTransform if it has.
        /// </summary>
        /// <returns>true if has rectTransform</returns>
        private bool SetAnchoredPosition(Vector3 anchoredPosition)
        {
            if (isRectTransform) rectTransform.anchoredPosition = anchoredPosition;
            return isRectTransform;
        }
        /// <summary>
        /// Get the anchored position of the RectTransform if it has.
        /// </summary>
        private Vector3 GetAnchoredPosition()
        {
            if (isRectTransform) return rectTransform.anchoredPosition;
            else return default;
        }
        /// <summary>
        /// Return main camera (<see cref="Camera.main"/>) initialized in Awake if it exists using out parameter
        /// </summary>
        /// <returns>if exists return true</returns>
        /// <remarks>- This function must be called after Start( )</remarks>
        public bool TryGetMainCamera(out Camera camera)
        {
            if (mainCamera == null) mainCamera = Camera.main;

            camera = mainCamera;
            return camera != null;
        }
        /// <summary>
        /// Checks if any of the vector elements returns NaN.
        /// <paramref name="vector"/>
        /// </summary>
        /// <param name="vector">Vector3 checked</param>
        /// <returns>
        /// - 'isNaN' will be true if any element is NaN.<br/>
        /// - 'safety' returns the Vector3 with all NaN elements converted to 0f.
        /// </returns>
        public (bool isNaN, Vector3 safety) Verify(Vector3 vector)
        {
            var isNaN_x = float.IsNaN(vector.x);
            var isNaN_y = float.IsNaN(vector.y);
            var isNaN_z = float.IsNaN(vector.z);
            var safety = new Vector3()
            {
                x = isNaN_x ? 0f : vector.x,
                y = isNaN_y ? 0f : vector.y,
                z = isNaN_z ? 0f : vector.z
            };

            return (isNaN_x || isNaN_y || isNaN_z, safety);
        }
        /// <summary>
        /// Checks if any of the vector elements returns NaN.
        /// <paramref name="quaternion"/>
        /// </summary>
        /// <param name="quaternion">Qauternion checked</param>
        /// <returns>
        /// - 'isNaN' will be true if any element is NaN.<br/>
        /// - 'safety' returns the Vector3 with all NaN elements converted to 0f.
        /// </returns>
        public (bool isNaN, Quaternion safety) Verify(Quaternion quaternion)
        {
            var isNaN_x = float.IsNaN(quaternion.x);
            var isNaN_y = float.IsNaN(quaternion.y);
            var isNaN_z = float.IsNaN(quaternion.z);
            var isNaN_w = float.IsNaN(quaternion.w);
            var safety = new Quaternion()
            {
                x = isNaN_x ? 0f : quaternion.x,
                y = isNaN_y ? 0f : quaternion.y,
                z = isNaN_z ? 0f : quaternion.z,
                w = isNaN_w ? 0f : quaternion.w
            };

            return (isNaN_x || isNaN_y || isNaN_z || isNaN_w, safety);
        }
        /// <summary>
        /// Set Rect Values
        /// </summary>
        private void UpdateValues()
        {
            if (isRectTransform)
            {
                rect = rectTransform.rect;
                ratio = rect.width / rect.height;
                result = rangeRatioHandler * ratio;
            }
#if UNITY_EDITOR
            AssignValuesToInspector();
#endif
        }
        /// <summary>
        /// Set Range Ratio Handler <paramref name="value"/>
        /// </summary>
        /// <remarks>
        /// - The minimum value of <paramref name="value"/> must be 0.001f or will apply this minimum value!<br/>
        /// - The values will be updated!
        /// </remarks>
        public void SetRangeRatioHandler(float value)
        {
            if (value < 0.001f) value = 0.001f;

            rangeRatioHandler = value;
            UpdateValues();
        }
        /// <summary>
        /// Return Rect Trabsform if it has
        /// </summary>
        public RectTransform RectTransform => rectTransform;
        /// <summary>
        /// Get or Set local scale
        /// </summary>
        /// <remarks>- If any of the float variables return NaN, it will be converted to 0f.</remarks>
        public Vector3 Scale
        {
            get => Verify(transform.localScale).safety;
            set => transform.localScale = Verify(value).safety;
        }
        /// <summary>
        /// Get or Set Position
        /// </summary>
        /// <remarks>- If any of the float variables return NaN, it will be converted to 0f.</remarks>
        public Vector3 Position
        {
            get => Verify(transform.position).safety;
            set => transform.position = Verify(value).safety;
        }
        /// <summary>
        /// Get or Set Local Position
        /// </summary>
        /// <remarks>- If any of the float variables return NaN, it will be converted to 0f.</remarks>
        public Vector3 LocalPosition
        {
            get => Verify(transform.localPosition).safety;
            set => transform.localPosition = Verify(value).safety;
        }
        /// <summary>
        /// Get or Set Anchored Position if it has
        /// </summary>
        /// <remarks>- If any of the float variables return NaN, it will be converted to 0f.</remarks>
        public Vector3 AnchoredPosition
        {
            get => Verify(GetAnchoredPosition()).safety;
            set => SetAnchoredPosition(Verify(value).safety);
        }
        /// <summary>
        /// Get or Set Rotation
        /// </summary>
        /// <remarks>- If any of the float variables return NaN, it will be converted to 0f.</remarks>
        public Quaternion Rotation
        {
            get => Verify(transform.rotation).safety;
            set => transform.rotation = Verify(value).safety;
        }
        /// <summary>
        /// Get or Set local rotation
        /// </summary>
        /// <remarks>- If any of the float variables return NaN, it will be converted to 0f.</remarks>
        public Quaternion LocalRotation
        {
            get => Verify(transform.localRotation).safety;
            set => transform.localRotation = Verify(value).safety;
        }
        /// <summary>
        /// Get or Set Euler Angles
        /// </summary>
        /// <remarks>- If any of the float variables return NaN, it will be converted to 0f.</remarks>
        public Vector3 EulerAngles
        {
            get => Verify(transform.eulerAngles).safety;
            set => transform.eulerAngles = Verify(value).safety;
        }
        /// <summary>
        /// Get or Set Local Euler Angles
        /// </summary>
        /// <remarks>- If any of the float variables return NaN, it will be converted to 0f.</remarks>
        public Vector3 LocalEulerAngles
        {
            get => Verify(transform.localEulerAngles).safety;
            set => transform.localEulerAngles = Verify(value).safety;
        }
        /// <summary>
        /// Get or Set Camera Screen To World Point from Position
        /// </summary>
        /// <remarks>- If any of the float variables return NaN, it will be converted to 0f.</remarks>
        public Vector3 ScreenToWorldPointPosition
        {
            get
            {
                if (TryGetMainCamera(out _)) return Verify(mainCamera.ScreenToWorldPoint(transform.position)).safety;
                else return Vector3.zero;
            }

            set
            {
                if (TryGetMainCamera(out _)) transform.position = mainCamera.ScreenToWorldPoint(Verify(value).safety);
                else transform.position = Vector3.zero;
            }
        }
        /// <summary>
        /// Get or Set Camera Screen To World Point from Local Position
        /// </summary>
        /// <remarks>- If any of the float variables return NaN, it will be converted to 0f.</remarks>
        public Vector3 ScreenToWorldPointLocalPosition
        {
            get
            {
                if (TryGetMainCamera(out _)) return Verify(mainCamera.ScreenToWorldPoint(transform.localPosition)).safety;
                else return Vector3.zero;
            }

            set
            {
                if (TryGetMainCamera(out _)) transform.localPosition = mainCamera.ScreenToWorldPoint(Verify(value).safety);
                else transform.localPosition = Vector3.zero;
            }
        }
        /// <summary>
        /// Get or Set Camera Screen To World Point from Anchored Position if it has
        /// </summary>
        /// <remarks>- If any of the float variables return NaN, it will be converted to 0f.</remarks>
        public Vector3 ScreenToWorldPointAnchoredPosition
        {
            get
            {
                if (TryGetMainCamera(out _)) return Verify(mainCamera.ScreenToWorldPoint(GetAnchoredPosition())).safety;
                else return Vector3.zero;
            }

            set
            {
                if (TryGetMainCamera(out _)) SetAnchoredPosition(mainCamera.ScreenToWorldPoint(Verify(value).safety));
                else SetAnchoredPosition(Vector3.zero);
            }
        }
        /// <summary>
        /// Returns Rect from <see cref="RectTransform.rect"/>
        /// </summary>
        public Rect Rect => rect;
        /// <summary>
        /// Returns a ratio calculation between <see cref="RectTransform.rect"/>.width and <see cref="RectTransform.rect"/>.height.
        /// </summary>
        /// <remarks>- If the ratio is NaN or Infinity, it will return null.</remarks>
        public float? Ratio
        {
            get
            {
                if (float.IsNaN(ratio) || float.IsInfinity(ratio)) return null;
                else return ratio;
            }
        }
        /// <summary>
        /// Returns the result of the multiplication between <see cref="RangeRatioHandler"/> and <see cref="Ratio"/>.ratio.
        /// </summary>
        /// <remarks>- If the ratio is NaN or Infinity, it will return null.</remarks>
        public float? RangeRatioResult
        {
            get
            {
                if (float.IsNaN(result) || float.IsInfinity(result)) return null;
                else return result;
            }
        }
        /// <summary>
        /// Does the Game Object have a RectTransform?
        /// </summary>
        public bool IsRectTransform => isRectTransform;
        /// <summary>
        /// Returns the value of range ratio Handler
        /// </summary>
        /// <remarks>- If you want to assign the value to <see cref="RangeRatioHandler"/> use <see cref="SetRangeRatioHandler(float)"/>.</remarks>
        public float RangeRatioHandler => rangeRatioHandler;
    }
}