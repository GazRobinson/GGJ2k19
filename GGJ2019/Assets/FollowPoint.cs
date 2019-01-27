﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPoint : MonoBehaviour {
    public Transform target;

	void Update () {
        transform.position = target.position;
        transform.rotation = target.rotation;
	}
}
