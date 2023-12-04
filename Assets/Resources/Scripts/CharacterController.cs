using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class CharacterSpriteSelector : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private float frameRate = 12f;
    
    public enum CharacterName
    {
        Activist, Artist, Child, Cook, Janitor, Maid, Manager, Porter, Student, Teacher
    }
    public CharacterName characterName;
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        
        Sprite[] sprites = Resources
            .LoadAll<Sprite>("Sprites/Characters")
            .Where(sprite => 
                sprite.name.StartsWith(characterName.ToString().ToLower()))
            .OrderBy(sprite => int.Parse(sprite.name.Split("_")[1]))
            .ToArray();
        
        _animator = gameObject.AddComponent<Animator>();
        var controller = new AnimatorController();
        _animator.runtimeAnimatorController = controller;
        Debug.Log(controller);
        controller.AddParameter("IsWalking", AnimatorControllerParameterType.Bool);
        controller.AddParameter("X", AnimatorControllerParameterType.Float);
        controller.AddParameter("Y", AnimatorControllerParameterType.Float);

        controller.AddLayer("BaseLayer");
        
        var rootStateMachine = controller.layers[0].stateMachine;
        var stateMachine = rootStateMachine.AddStateMachine("stateMachine");

        var idleState = new AnimatorState();
        var idleTree = new BlendTree();
        idleState.motion = idleTree;
        idleTree.blendParameter = "X";
        idleTree.blendParameterY = "Y";
        idleTree.name = "Idle Tree";
        idleTree.blendType = BlendTreeType.SimpleDirectional2D;
        var walkState = new AnimatorState();
        var walkTree = new BlendTree();
        walkState.motion = walkTree;
        walkTree.blendParameter = "X";
        walkTree.blendParameterY = "Y";
        walkTree.name = "Walk Tree";
        walkTree.blendType = BlendTreeType.SimpleDirectional2D;
        
        stateMachine.AddState(idleState, Vector2.zero);
        stateMachine.AddState(walkState, Vector2.zero);

        var idleWalkTransition = idleState.AddTransition(walkState);
        var walkIdleTransition = walkState.AddTransition(idleState);
        idleWalkTransition.AddCondition(AnimatorConditionMode.If, 0f, "IsWalking");
        walkIdleTransition.AddCondition(AnimatorConditionMode.IfNot, 0f, "IsWalking");
        
        idleTree.AddChild(createAnimation(new Sprite[]{ sprites[1] }), new Vector2(0f, -1f));
        idleTree.AddChild(createAnimation(new Sprite[]{ sprites[10] }), new Vector2(0f, 1f));
        idleTree.AddChild(createAnimation(new Sprite[]{ sprites[7] }), new Vector2(0.5f, 0f));
        idleTree.AddChild(createAnimation(new Sprite[]{ sprites[4] }), new Vector2(-0.5f, 0f));
        walkTree.AddChild(createAnimation(new Sprite[]{ sprites[9], sprites[10], sprites[11] }), new Vector2(0f, 1f));
        walkTree.AddChild(createAnimation(new Sprite[]{ sprites[6], sprites[7],  sprites[8] }), new Vector2(0.5f, 0f));
        walkTree.AddChild(createAnimation(new Sprite[]{ sprites[0], sprites[1],  sprites[2] }), new Vector2(0f, -1f)); 
        walkTree.AddChild(createAnimation(new Sprite[]{ sprites[3], sprites[4],  sprites[5] }), new Vector2(-0.5f, 0f));

    }
    
    void Update()
    {
        Vector2 movement_vector = _rigidBody.velocity;
        if (movement_vector.x != 0 || movement_vector.y != 0)
        {
            _animator.SetFloat("X", movement_vector.x);
            _animator.SetFloat("Y", movement_vector.y);
            
            _animator.SetBool("IsWalking", true);            
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
    }
    
    void OnApplicationQuit()
    {
        AnimatorController controller = _animator.runtimeAnimatorController as AnimatorController;
    }

    private AnimationClip createAnimation(Sprite[] frames)
    {
        AnimationClip animationClip = new AnimationClip();

        animationClip.frameRate = frameRate;

        EditorCurveBinding spriteBinding = new EditorCurveBinding();
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_Sprite";

        ObjectReferenceKeyframe[] spriteKeyframes = new ObjectReferenceKeyframe[frames.Length];

        for (int i = 0; i < frames.Length; i++)
        {
            spriteKeyframes[i] = new ObjectReferenceKeyframe();
            spriteKeyframes[i].time = i / frameRate;
            spriteKeyframes[i].value = frames[i];
        }

        AnimationUtility.SetObjectReferenceCurve(animationClip, spriteBinding, spriteKeyframes);
        
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(animationClip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(animationClip, settings);
        
        return animationClip;
    }
}
