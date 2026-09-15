using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class UnitTest_PlayerData : MonoBehaviour
{
    [SerializeField]
    private CharacterData playerData;

    private Animator animator;
    private new Rigidbody2D rigidbody2D;
 
    private float horizontalInput;
    private bool bFacingRight = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

#if UNITY_EDITOR
    private void Reset()
    {
        string path = "Assets/UnitTests/01_CharacterData/PlayerData.asset";
        playerData = AssetDatabase.LoadAssetAtPath<CharacterData>(path);
    }
#endif

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("MoveSpeed", Mathf.Abs(horizontalInput));

        if (horizontalInput > 0.0f && bFacingRight == false)
            Flip();
        else if (horizontalInput < 0.0f && bFacingRight)
            Flip();
    }

    private void FixedUpdate()
    {
        rigidbody2D.linearVelocity = new Vector2(horizontalInput * playerData.MoveSpeed, rigidbody2D.linearVelocity.y);
    }

    private void Flip()
    {
        bFacingRight = !bFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1.0f;
        transform.localScale = scale;
    }

    private void OnGUI()
    {
        GUILayout.Label(horizontalInput.ToString());
    }
}
