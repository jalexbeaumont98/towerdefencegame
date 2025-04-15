using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyStatusBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject bar;
    [SerializeField] GameObject statusIconPrefab;
    [SerializeField] List<GameObject> statusIconPool;
    [SerializeField] List<EnemyStatusData> extraStatuses;
    [SerializeField] private EnemyStatus fullstatus;
    [SerializeField] private GameObject fullIconGameObject;
    [SerializeField] private EnemyBase enemy;


    [Header("Attributes")]
    [SerializeField] private int maxStatuses;
    [SerializeField] private int statusCount = 0;

    private bool isFull;

    private Dictionary<string, EnemyStatusData> statusAmounts = new Dictionary<string, EnemyStatusData>();


    void Awake()
    {

        extraStatuses = new List<EnemyStatusData>();
        statusIconPool = new List<GameObject>();

    }




    public void Initialize(EnemyBase e)
    {
        enemy = e;

        enemy.OnStatusAdded += AddStatusIcon;
        enemy.OnStatusRemoved += RemoveStatusIcon;
    }

    private void AddStatusIcon(EnemyStatusData statusData)
    {
        /*
            If status already shown and stackable:
                Find existing StatusIcon
                Increment count
            Else if currentIconCount < maxIcons:
                Create StatusIcon
                Add to visibleIcons
            Else:
                Add to overflowQueue
                Show or update maxIcon

        */

        if (statusAmounts.ContainsKey(statusData.status.type)) //if it contains the key (stackable check is handled by enemyBase script)
        {
            if (statusAmounts[statusData.status.type].icon != null) //this is the check to see if its showing (ie not in the excess status list)
            {
                statusAmounts[statusData.status.type].icon.GetComponent<StatusIcon>().SetText(statusData.stackCount); //set icon text
            }

            statusAmounts[statusData.status.type].stackCount = statusData.stackCount;

            return;

        }

        else if (statusCount < maxStatuses)
        {
            GameObject statusIcon;

            statusIcon = GetFromPool();

            statusCount++;
            statusIcon.GetComponent<StatusIcon>().SetIcon(statusData.status.sprite);

            statusData.icon = statusIcon;
            statusAmounts.Add(statusData.status.type, statusData); //add to status dict
            
        }

        else
        {

            extraStatuses.Add(statusData); //add the statusData to the extra statuses list
            statusAmounts.Add(statusData.status.type, statusData); //also add to status dict

        }

        //SetMax();



    }



    private void RemoveStatusIcon(EnemyStatusData statusData)
    {
        /*

            Find matching StatusIcon
            If stackable and count > 1:
                If a visible icon:
                    Update text
                Decrement count
            Else:
                If a visible icon:
                    Remove and destroy icon
                    If overflowQueue not empty:
                        Dequeue and add to visible
                        Update maxIcon if needed
                Remove from status dict
                Remove from excess list

        */

        if (!statusAmounts.ContainsKey(statusData.status.type)) return;

        GameObject icon = statusAmounts[statusData.status.type].icon;

        if (statusData.stackCount > 0)
        {
            if (icon != null)
            {
                statusAmounts[statusData.status.type].icon.GetComponent<StatusIcon>().SetText(statusData.stackCount);
            }

            statusAmounts[statusData.status.type].stackCount = statusData.stackCount;
        }

        else
        {
            if (icon != null) //recall that this also serves as a check for whether or not the status is a visible one or in the excess list
            {
                AddBackToPool(icon);
                statusCount--;

                SetMax();
            }

            else
            {
                EnemyStatusData statusDataToRemove = null;

                foreach (EnemyStatusData estatusData in extraStatuses)
                {
                    if (estatusData.status.type == statusData.status.type) statusDataToRemove = estatusData;
                }

                if (statusDataToRemove != null) extraStatuses.Remove(statusDataToRemove);
            }

            statusAmounts.Remove(statusData.status.type);

        }


    }

    private void SetMax()
    {
        //If statusCount >= maxStatuses
        //instantiate maxIcon

        if (statusCount >= maxStatuses)
        {

            SpawnMaxIcon();
        }

        else
        {

            if (extraStatuses.Count > 0) 
            {
                EnemyStatusData newStatusData = extraStatuses[0];
                extraStatuses.Remove(newStatusData);
                AddStatusIcon(newStatusData);
            }

            else
            {
                if (fullIconGameObject != null) AddBackToPool(fullIconGameObject);
            }



        }

    }

    private void SpawnMaxIcon()
    {
        if (fullIconGameObject != null)
        {
            fullIconGameObject.GetComponent<StatusIcon>().SetText(extraStatuses.Count);
        }

        else
        {
            fullIconGameObject = GetFromPool();
            fullIconGameObject.GetComponent<StatusIcon>().SetIcon(fullstatus.sprite);
            fullIconGameObject.GetComponent<StatusIcon>().SetText(extraStatuses.Count);
            fullIconGameObject.transform.SetAsLastSibling();

        }
    }

    private void OnDestroy()
    {

        enemy.OnStatusAdded -= AddStatusIcon;
        enemy.OnStatusRemoved -= RemoveStatusIcon;
    }

    private GameObject GetFromPool()
    {

        GameObject go = null;

        if (statusIconPool.Count > 0) //get from pool if you can
        {
            go = statusIconPool[0];
            statusIconPool.RemoveAt(0);
            go.SetActive(true);
        }

        else //otherwise make a new one, itll get added the pool later
        {
            go = Instantiate(statusIconPrefab, bar.transform);
        }

        return go;
    }

    private void AddBackToPool(GameObject go)
    {
        go.SetActive(false);

        statusIconPool.Add(go);
    }


}
