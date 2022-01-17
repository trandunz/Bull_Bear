using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Rotator : MonoBehaviour
{
    bool m_Rotating = false;
    int m_RotationSign = 1;

    void Update()
    {
        if (m_Rotating)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + (1 * m_RotationSign), transform.rotation.z);
        }
    }

    public IEnumerator RotateLeft()
    {
        m_Rotating = true;
        m_RotationSign = -1;
        yield return new WaitForSeconds(2);
        m_Rotating = false;
    }
    public IEnumerator RotateRight()
    {
        m_Rotating = true;
        m_RotationSign = 1;
        yield return new WaitForSeconds(2);
        m_Rotating = false;
    }
}
