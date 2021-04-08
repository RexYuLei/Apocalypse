using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileController : MonoBehaviour
{
    private float mTouchIntervalTime = 1;
    private float mCoutTime;
    
    public Vector3 TouchStartPos { get; set; }
    public Vector3 OffsetPos { get; set; }
    
    private bool mCanControl = true;
    public float mTurnSpeed = 0.5f;

    private Character mCharacter;


    
    void Start()
    {
        mCharacter = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        ControlMove();
    }

    void ControlMove()
    {
        mCoutTime -= Time.deltaTime;
        if (mCoutTime <= 0)
        {
            mCoutTime = mTouchIntervalTime;
            TouchStartPos = (TouchStartPos + Input.mousePosition) / 2f;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            TouchStartPos = Input.mousePosition;
            mCoutTime = mTouchIntervalTime;
        }
        else if (Input.GetMouseButton(0))
        {
            if (this.mCanControl)
            {
                if (Vector3.Distance(Input.mousePosition, TouchStartPos) > 1)
                {
                    OffsetPos = Vector3.Normalize(Input.mousePosition - TouchStartPos);

                    Vector3 rDir = new Vector3(this.OffsetPos.x, 0, OffsetPos.y);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rDir, Vector3.up),
                        mTurnSpeed);
                }
            }

            mCharacter.AutoMove();
        }
        else
        {
            mCharacter.StopMove();
        }
    }
}
