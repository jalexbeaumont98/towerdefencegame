using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyStatusBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject bar;
    [SerializeField] GameObject statusIconPrefab;
    [SerializeField] List<GameObject> statusIcons;
    [SerializeField] List<EnemyStatus> extraStatuses;
    [SerializeField] private EnemyStatus fullstatus;


    [Header("Attributes")]
    [SerializeField] private int maxStatuses;

    private bool isFull;


    void Start()
    {
        statusIcons = new List<GameObject>();
        extraStatuses = new List<EnemyStatus>();

        
    }


    public void AddStatusIcon(EnemyStatus status)
    {
        if (statusIcons.Count <= maxStatuses)
        {
            GameObject newIcon = Instantiate(statusIconPrefab, bar.transform);
            statusIconPrefab.GetComponent<StatusIcon>().SetIcon(status);
            statusIcons.Add(newIcon);
        }

        else
        {
            extraStatuses.Add(status);
            SetBarFull();
        }


    }

    public bool RemoveStatusIcon(EnemyStatus status, int amount = -1)
    {
        GameObject statusIcon = null;

        foreach (GameObject go in statusIcons)
        {
            if (status.type == go.GetComponent<StatusIcon>().status.type)
            {
                print("found status icon");
                statusIcon = go;
            }
        }

        if (statusIcon != null)
        {
            if (amount > 0)
            {
                print("amount so no delete!");
                statusIcon.GetComponent<StatusIcon>().SetText(amount);
            }

            else
            {
                statusIcons.Remove(statusIcon);
                Destroy(statusIcon);

                print("icon should be deleted");
                SetBarFull();

            }

            return true;
        }

        EnemyStatus status_ = null;

        foreach (EnemyStatus exstatus in extraStatuses)
        {
            if (exstatus.type == status.type)
            {
                status_ = exstatus;
            }

        }

        if (status_ != null)
        {
            extraStatuses.Remove(status_);
            SetBarFull();
            return true;
        }



        return false;


    }

    public void StackAttribute(EnemyStatus status, int amount)
    {
        foreach (GameObject go in statusIcons)
        {
            EnemyStatus statusItem = go.GetComponent<StatusIcon>().status;

            if (statusItem.type == status.type)
            {
                go.GetComponent<StatusIcon>().SetText(amount);
            }
        }
    }

    private void SetBarFull()
    {
        if (statusIcons.Count > maxStatuses)
        {
            if (isFull) return;


            GameObject newIcon = Instantiate(statusIconPrefab, bar.transform);
            statusIconPrefab.GetComponent<StatusIcon>().SetIcon(fullstatus);

            isFull = true;

        }

        else
        {
            if (!isFull) return;

            GameObject go = statusIcons[maxStatuses];
            statusIcons.RemoveAt(maxStatuses);
            Destroy(go);

            if (extraStatuses.Count > 0)
            {
                AddStatusIcon(extraStatuses[0]);
                extraStatuses.RemoveAt(0);
            }

            isFull = false;

        }
    }


}
