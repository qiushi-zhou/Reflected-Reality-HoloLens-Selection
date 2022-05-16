using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransmitMessage : AbsMessage
{
    public float selection_time { get; set; }
    public Vector3 target_location { get; set; }
    public Vector3 selection_location { get; set; }
}
