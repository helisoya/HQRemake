using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a 3D character that is animated
/// </summary>
public class Character : MonoBehaviour
{
    [Header("Infos")]
    public string characterName;
    [SerializeField] private Transform root;
    private string pathToAnimations { get { return "Animations/Characters/" + characterName + "/"; } }

    [Header("Animators")]
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator eyeAnimator;
    [SerializeField] private Animator mouthAnimator;

    [Header("Fading")]
    [SerializeField] private float fadeSpeed;
    [SerializeField] private Renderer[] renderers;

    [Header("Interaction")]
    [SerializeField] private InteractableObject interactableObject;

    private List<Material> materials;
    private Coroutine fadingRoutine;
    private bool speaking = false;
    public RuntimeAnimatorController nextBodyRuntimeAnimatorController { get; private set; }
    public bool fading { get { return fadingRoutine != null; } }

    public Vector3 characterPosition { get { return transform.position; } }
    public float characterRotation { get { return transform.eulerAngles.y; } }
    public string bodyAnimationSet { get; private set; }
    public string eyeAnimationSet { get; private set; }
    public string mouthAnimationSet { get; private set; }
    public float characterAlpha { get; private set; }

    void Awake()
    {
        RegisterMaterials();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeBodyAnimation("Mug");
        }
    }

    public void UnregisterInteraction()
    {
        interactableObject.Unregister();
    }

    /// <summary>
    /// Registers the renderer's materials
    /// </summary>
    private void RegisterMaterials()
    {
        if (materials == null) materials = new List<Material>();
        else materials.Clear();

        foreach (Renderer renderer in renderers)
        {
            materials.Add(renderer.material);
        }
    }


    /// <summary>
    /// Changes the position of the character
    /// </summary>
    /// <param name="position">The character's position</param>
    public void SetPosition(Vector3 position)
    {
        root.position = position;
    }

    /// <summary>
    /// Changes the rotation of the character
    /// </summary>
    /// <param name="rotation">The character's rotation</param>
    public void SetRotation(float rotation)
    {
        root.eulerAngles = new Vector3(0, rotation, 0);
    }

    /// <summary>
    /// Sets the character's alpha
    /// </summary>
    /// <param name="value">The new alpha value</param>
    public void SetAlpha(float value, bool changeGlobalValue = false)
    {
        if (changeGlobalValue) characterAlpha = value;

        foreach (Material material in materials)
        {
            material.SetFloat("_Alpha", value);
        }
    }


    /// <summary>
    /// Changes if the character is speaking or not
    /// </summary>
    /// <param name="value">Is the character speaking ?</param>
    public void SetTalking(bool value)
    {
        if (value == speaking) return;

        speaking = value;

        mouthAnimator.SetBool("Speak", speaking);
        bodyAnimator.SetBool("Speak", speaking);
    }

    /// <summary>
    /// Starts fading the character
    /// </summary>
    /// <param name="value">Target alpha (0 for invisble, 1 for visible)</param>
    /// <param name="force">Proceed even if there is already a fading in progress ? (true by default)</param>
    public void FadeTo(float value, bool force = true)
    {
        if (fadingRoutine != null)
        {
            if (!force) return;
            StopCoroutine(fadingRoutine);
        }

        fadingRoutine = StartCoroutine(RoutineFading(value));
    }

    /// <summary>
    /// IEnumerator for the character's fading
    /// </summary>
    /// <param name="to">Target alpha</param>
    /// <returns>IEnumerator</returns>
    IEnumerator RoutineFading(float to)
    {
        characterAlpha = to;
        float currentAlpha = materials[0].GetFloat("_Alpha");
        float step = currentAlpha < characterAlpha ? 1 : -1;

        float boundMin = step == 1 ? 0 : to;
        float boundMax = step == 1 ? to : 1;

        while (currentAlpha != characterAlpha)
        {
            currentAlpha = Mathf.Clamp(currentAlpha + fadeSpeed * step * Time.deltaTime, boundMin, boundMax);
            SetAlpha(currentAlpha);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        fadingRoutine = null;
    }

    /// <summary>
    /// Changes the eye's animation set
    /// </summary>
    /// <param name="newAnimation">The new animation set's name</param>
    public void ChangeEyeAnimation(string newAnimation)
    {
        if (string.IsNullOrEmpty(newAnimation)) return;
        eyeAnimationSet = newAnimation;
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(pathToAnimations + "Eye/" + newAnimation);
        if (controller)
        {
            eyeAnimator.runtimeAnimatorController = controller;
        }
    }

    /// <summary>
    /// Changes the mouth's animation set
    /// </summary>
    /// <param name="newAnimation">The new animation set's name</param>
    public void ChangeMouthAnimation(string newAnimation)
    {
        if (string.IsNullOrEmpty(newAnimation)) return;
        mouthAnimationSet = newAnimation;
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(pathToAnimations + "Mouth/" + newAnimation);
        if (controller)
        {
            mouthAnimator.runtimeAnimatorController = controller;
        }
    }

    /// <summary>
    /// Changes the body's animation set
    /// </summary>
    /// <param name="newAnimation">The new animation set's name</param>
    /// <param name="immediate">Should the change be immediate ?</param>
    public void ChangeBodyAnimation(string newAnimation, bool immediate = false)
    {
        if (string.IsNullOrEmpty(newAnimation)) return;
        bodyAnimationSet = newAnimation;
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(pathToAnimations + "Body/" + newAnimation);
        if (controller)
        {

            if (immediate)
            {
                ChangeBodyAnimationSetImmediate(controller);
            }
            else
            {
                nextBodyRuntimeAnimatorController = controller;
                bodyAnimator.SetTrigger("Exit");
            }
        }
    }


    /// <summary>
    /// Changes the animator's controller
    /// </summary>
    /// <param name="controller">The new controller</param>
    public void ChangeBodyAnimationSetImmediate(RuntimeAnimatorController controller)
    {
        bodyAnimator.runtimeAnimatorController = controller;
        nextBodyRuntimeAnimatorController = null;
    }

}
