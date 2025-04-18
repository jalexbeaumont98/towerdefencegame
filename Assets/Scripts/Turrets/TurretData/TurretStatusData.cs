using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class TurretStatusData
{
    [JsonProperty("name")]
    public string name;

    [JsonProperty("attributes")]
    public Dictionary<string, string> attributes;
}
