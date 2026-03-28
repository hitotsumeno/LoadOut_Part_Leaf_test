using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Collider2D _feetColl;

    [Header("Ground")]
    private RaycastHit2D _groundHit;
    public bool _isGrounded {get; private set;}
    [SerializeField] private float groundDetectionRayLength;

    [SerializeField] private LayerMask groundLayer;

    [Header("Debug")]
    [SerializeField] private bool debugShowIsGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CollisionChecks();
    }
    #region Ground Check
    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.center.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, groundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, groundDetectionRayLength, groundLayer);
        if (_groundHit.collider != null)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        #region Debug Visualization
        if (debugShowIsGrounded)
        {
            Color rayColor;
            if (_isGrounded)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - groundDetectionRayLength), Vector2.right * boxCastOrigin, rayColor);
        }
        #endregion
    }

    private void CollisionChecks()
    {
        IsGrounded();
    }

    #endregion
}
