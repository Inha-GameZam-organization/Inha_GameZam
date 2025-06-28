using UnityEngine;
public enum ContactInfo
{
    LEFTWALL,
    RIGHTWALL,
    NONE,
}

public class PlayerWallCollisionCheck : MonoBehaviour
{
    [HideInInspector]
    public ContactInfo wallContacted = ContactInfo.NONE;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // ��Ȯ�� �� ���� ������ ���� x ���� �������� ��
            if (Mathf.Abs(contact.normal.x) > Mathf.Abs(contact.normal.y))
            {
                if (contact.normal.x > 0.5f)
                {
                    wallContacted = ContactInfo.LEFTWALL; // ���� ���� ����� �� (���� ���ʿ� ����)
                    return;
                }
                else if (contact.normal.x < -0.5f)
                {
                    wallContacted = ContactInfo.RIGHTWALL; // ������ ���� ����� �� (���� �����ʿ� ����)
                    return;
                }
            }
        }

        wallContacted = ContactInfo.NONE;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (wallContacted != ContactInfo.NONE)
        {
            wallContacted = ContactInfo.NONE;
        }
    }
}
