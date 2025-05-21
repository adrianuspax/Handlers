using System;
using UnityEngine;

namespace ASPax.Handlers
{
    /// <summary>
    /// Animator Handler
    /// </summary>
    [Serializable]
    public class AnimatorHandler
    {
        /// <summary>
        /// Struct for handling animation parameters
        /// </summary>
        [Serializable]
        public struct Parameter
        {
            [SerializeField] private string name;
            [SerializeField] private int id;
            /// <summary>
            /// Constructor for the struct to be instantiated
            /// </summary>
            /// <param name="name">Name of the parameter</param>
            public Parameter(string name)
            {
                this.name = name;
                id = Animator.StringToHash(name);
            }
            /// <summary>
            /// Constructor for the struct to be instantiated
            /// </summary>
            /// <param name="controllerParameter">Animator Controller Parameter</param>
            public Parameter(AnimatorControllerParameter controllerParameter)
            {
                name = controllerParameter.name;
                id = Animator.StringToHash(controllerParameter.name);
            }
            /// <summary>
            /// Returns the name of the parameter
            /// </summary>
            /// <remarks><see cref="ID"/> is recommended to use for into methods that need to access the animation parameter.</remarks>
            public readonly string Name => name;
            /// <summary>
            /// Return the parameter ID
            /// </summary>
            /// <remarks>It is recommended to use for the methods that need to access the animation parameter.</remarks>
            public readonly int ID => id;
        }

        [SerializeField] private Animator animator;
        [SerializeField, NonReorderable] private Parameter[] parameters;
        [SerializeField, NonReorderable] private AnimationClip[] clips;
        /// <summary>
        /// Constructor for the class to be instantiated
        /// </summary>
        /// <param name="animator">Animator Component</param>
        public AnimatorHandler(Animator animator)
        {
            if (animator == null)
            {
                IsInstanstiated = false;
                return;
            }

            this.animator = animator;
            parameters = new Parameter[animator.parameterCount];
            clips = animator.runtimeAnimatorController.animationClips;

            for (var i = 0; i < animator.parameterCount; i++)
                parameters[i] = new(animator.parameters[i]);

            IsInstanstiated = true;
        }
        /// <summary>
        /// Returns the Animator Component
        /// </summary>
        public Animator Animator => animator;
        /// <summary>
        /// Returns AnimatorControllerParameter array
        /// </summary>
        public AnimatorControllerParameter[] ControllerParameters => animator.parameters;
        /// <summary>
        /// Returns the parameters struct
        /// </summary>
        public Parameter[] Parameters => parameters;
        /// <summary>
        /// Returns the animation clips array
        /// </summary>
        public AnimationClip[] Clips => clips;
        /// <summary>
        /// Gets a value indicating whether the object has been instantiated.
        /// </summary>
        public bool IsInstanstiated { get; private set; }
    }
}