using Dreamteck.Splines;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RailSwitcher : MonoBehaviour
{
    [SerializeField] private SplineFollower follower;
    [SerializeField] private List<SplineComputer> rails;
    [SerializeField] private Transform cameraPosition;

    private int currentRailIndex = 1;
    private bool isActive = false;

    private void Start()
    {
        GameManager.Instance.RegisterRailSwitcher(this);
        GameManager.Instance.GetPlayerController().SetRailSwitcher(this);
    }

    private void TrySwitch(int direction)
    {
        if (!isActive)
        {
            return;
        }

        int newIndex = currentRailIndex + direction;

        if (newIndex >= 0 && newIndex < rails.Count)
        {
            double percent = follower.GetPercent();
            follower.spline = rails[newIndex];
            follower.SetPercent(percent);
            currentRailIndex = newIndex;
        }
    }

    public void SwitchLeft()
    {
        TrySwitch(-1);
    }

    public void SwitchRight()
    {
        TrySwitch(1);
    }

    public Vector3 GetCameraPosition()
    {
        return cameraPosition.position;
    }

    public void SetActive()
    {
        isActive = true;
    }
}
