using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;


public class BodySourceView : MonoBehaviour 
{
    public Material BoneMaterial;
    public GameObject BodySourceManager;
    
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;
    
    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };

    private List<GestureData> playerGestureDatas = new List<GestureData>();

    public enum Gesture
    {
        None = 0,
        RaiseRightHand,
        RaiseLeftHand,
    }

    public class GestureData
    {
        public ulong userId;
        public uint state;
        public float duration;
        public Gesture gesture;
        public float timestamp;

        public GestureData(ulong id, uint state, float duration, Gesture gesture)
        {
            this.userId = id;
            this.state = state;
            this.duration = duration;
            this.gesture = gesture;
            this.timestamp = 0f;
        }

        public void SetGestureTracking(float timestamp)
        {
            this.state = 1;
            this.timestamp = timestamp;
        }

        public void CheckGestureComplete(float timestamp, bool isInPose)
        {
            if (isInPose)
            {
                float timeLeft = timestamp - this.timestamp;

                if (timeLeft >= this.duration)
                {
                    Debug.Log("id: " + this.userId + "\nGesture: " + this.gesture);
                    this.timestamp = timestamp;
                    this.state = 1;
                }
            }
            else
            {
                SetGestureCancelled();
            }
        }

        public void SetGestureCancelled()
        {
            this.state = 0;
        }
    }

    void Update () 
    {
        //check all components
        if (BodySourceManager == null)
        {
            return;
        }
        
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }
        
        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }
        
        //add tracker ids
        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
              }
                
            if(body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }
        
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
        
        // First delete untracked bodies
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
                RemoveGestureDatas(trackingId);
            }
        }

        //create body object
        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
            }
            
            if(body.IsTracked)
            {
                if(!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                    AddGestureDatas(body.TrackingId);
                }
                
                //RefreshBodyObject(body, _Bodies[body.TrackingId]);
                ProcessSkeleton(body);
            }
        }
    }

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);

/*        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            *//*LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.material = BoneMaterial;
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;

            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;*//*
        }*/

        return body;
    }

    /*private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }

            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if (targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObj.localPosition);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                lr.startColor = GetColorForState(sourceJoint.TrackingState);
                lr.endColor = GetColorForState(targetJoint.Value.TrackingState);
            }
            else
            {
                lr.enabled = false;
            }
        }
    }

    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
            case Kinect.TrackingState.Tracked:
                return Color.green;

            case Kinect.TrackingState.Inferred:
                return Color.red;

            default:
                return Color.black;
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }*/

    private void AddGestureDatas(ulong id)
    {
        Debug.Log("add: " + id);
        GestureData raiseLeftHandGestureData = new GestureData(id, 0, 2f, Gesture.RaiseLeftHand);
        GestureData raiseRightHandGestureData = new GestureData(id, 0, 2f, Gesture.RaiseRightHand);
        playerGestureDatas.Add(raiseLeftHandGestureData);
        playerGestureDatas.Add(raiseRightHandGestureData);
    }

    private void RemoveGestureDatas(ulong id)
    {
        Debug.Log("remove: " + id);
        List<GestureData> copyGestureDatas = new List<GestureData>(playerGestureDatas);
        foreach (var gestureData in copyGestureDatas)
        {
            if (gestureData.userId == id)
                playerGestureDatas.Remove(gestureData);
        }
    }

    public void ProcessSkeleton(Kinect.Body body)
    {
        // get body id
        ulong id = body.TrackingId;

        //check for each gesture
        foreach (var gestureData in playerGestureDatas)
        {
            if (id != gestureData.userId) return;

            switch (gestureData.gesture)
            {
                case Gesture.RaiseRightHand:
                    Kinect.Joint rightHand = body.Joints[Kinect.JointType.HandRight];
                    Kinect.Joint rightShoulder = body.Joints[Kinect.JointType.ShoulderRight];
                    bool raiseRightHand = (rightHand.Position.Y - rightShoulder.Position.Y) > 0.1f;
                    switch (gestureData.state)
                    {
                        //detection
                        case 0:
                            if (raiseRightHand)
                                gestureData.SetGestureTracking(Time.time);
                            break;
                        //completion
                        case 1:
                            gestureData.CheckGestureComplete(Time.time, raiseRightHand);
                            break;
                    }
                    break;

                case Gesture.RaiseLeftHand:
                    Kinect.Joint leftHand = body.Joints[Kinect.JointType.HandLeft];
                    Kinect.Joint leftShoulder = body.Joints[Kinect.JointType.ShoulderLeft];
                    bool raiseLeftHand = (leftHand.Position.Y - leftShoulder.Position.Y) > 0.1f;
                    switch (gestureData.state)
                    {
                        case 0:
                            if (raiseLeftHand)
                                gestureData.SetGestureTracking(Time.time);
                            break;
                        case 1:
                            gestureData.CheckGestureComplete(Time.time, raiseLeftHand);
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
