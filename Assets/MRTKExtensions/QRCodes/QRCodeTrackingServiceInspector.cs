#if UNITY_EDITOR
using System;
using Microsoft.MixedReality.Toolkit.Editor;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;
using UnityEditor;

namespace MRTKExtensions.QRCodes.Editor
{	
	[MixedRealityServiceInspector(typeof(IQRCodeTrackingService))]
	public class QRCodeTrackingServiceInspector : BaseMixedRealityServiceInspector
	{
		public override void DrawInspectorGUI(object target)
		{
			QRCodeTrackingService service = (QRCodeTrackingService)target;
			
			// Draw inspector here
		}
	}
}

#endif