using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CustomEvents
{
    public static Action StartSpin;
    public static Action<int> StopReelSpin;
    public static Action SpinFullStop;
    public static Action<float> UpdateCoins;
    public static Action<float, float> UpdateBets;
    public static Action<int> Winner;
    public static Action<float> GainWinnings;
}
