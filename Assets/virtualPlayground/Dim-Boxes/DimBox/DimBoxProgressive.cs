using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace DimBoxes
{
    [ExecuteInEditMode]

    public class DimBoxProgressive : DimBox
    {
        private bool coroutineRunning = false;
        [Range(0, 1)]
        public float boxProgress = 0.5f;
        private Vector3[] animationEndPoints = new Vector3[12];

        protected override void SetLines()
        {

            List<Vector3[]> _lines = new List<Vector3[]>();
            if (boxProgress < 1)
            {
                DoEndPoints();
            }

            //box lines
            Vector3[] _line;
            Vector3 endpoint;
            for (int i = 0; i < 4; i++)
            {
                //bottom rect
                endpoint = (boxProgress < 1) ? animationEndPoints[i] : corners[4 + (i + 1) % 4];
                _line = new Vector3[] { corners[4 + i], endpoint };
                _lines.Add(_line);
                //height
                endpoint = (boxProgress < 1) ? animationEndPoints[i + 4] : i % 2 * corners[i] + (i + 1) % 2 * corners[i + 4];
                _line = new Vector3[] { i % 2 * corners[i + 4] + (i + 1) % 2 * corners[i], endpoint };
                _lines.Add(_line);
                //top rect
                endpoint = (boxProgress < 1) ? animationEndPoints[i + 8] : corners[(i + 1) % 4];
                _line = new Vector3[] { corners[i], endpoint };
                _lines.Add(_line);
            }

            //*/
            triangles = new Vector3[0][];
            if (boxProgress >= 1)
            {
                DoExtensionsAndTriangles(_lines);
            }
            lines = new Vector3[_lines.Count, 2];
            for (int j = 0; j < _lines.Count; j++)
            {
                lines[j, 0] = _lines[j][0];
                lines[j, 1] = _lines[j][1];
            }

        }

       /* void OnMouseDown()
        {
            if (!interactive) return;
            enabled = !enabled;
            hDimensionMesh.SetActive(enabled);
            dDimensionMesh.SetActive(enabled);
            wDimensionMesh.SetActive(enabled);
        }*/

        public void Animate(float val, int mode)
        {
            if (coroutineRunning) return;
            //reset shaders
            lineMaterial.SetFloat("_Animated", 0);
            lineMaterial.DisableKeyword("_ANIMATED_CENTRAL");
            lineMaterial.DisableKeyword("_ANIMATED_DIAGONAL_CORNER");
            lineMaterial.DisableKeyword("_ANIMATED_DIAGONAL_CENTRE");
            //
            if (mode != 1)
            {
                boxProgress = 1;
            }

            if (mode == 0 || val == 0)
            {
                enabled = !enabled;
                return;
            }

            if (mode == 2 || mode == 3 || mode == 4 || mode == 5)
            {
                lineMaterial.SetVector("_Centre", transform.TransformPoint(boundOffset));
            }

            if (mode == 2 || mode == 3)
            {
                lineMaterial.SetVector("_BoxDirZ", transform.forward);
                lineMaterial.SetVector("_BoxDirY", transform.up);
                lineMaterial.SetVector("_BoxDirX", transform.right);
            }

            if (mode == 3 || mode == 5)
            {
                lineMaterial.SetFloat("_inverse", 1);
            }
            else
            {
                lineMaterial.SetFloat("_inverse", 0);
            }
            lineMaterial.SetVector("_BoxExtent", bound.extents);
            int k = 0;

            if (mode == 4 || mode == 5 || mode == 6)
            {
                k = SortCornerForDiagonalAnimation();
                int l = (k + 2) % 4;
                //Debug.Log(k.ToString() + " | " + l.ToString());
                Vector3 diagPlane = transform.TransformPoint(corners[(k + 2) % 4]) - transform.TransformPoint(corners[k]);
                lineMaterial.SetVector("_DiagPlane", diagPlane.normalized);
            }

            if (mode == 6)
            {
                lineMaterial.SetVector("_Centre", transform.TransformPoint(corners[k]));
            }

            StopAllCoroutines();
            //if (val != 0) ;
            bool enabledAtStart = enabled;
            StartCoroutine(Fade(enabledAtStart, val, mode));
        }

        private IEnumerator Fade(bool val, float speed, int mode)
        {
            coroutineRunning = true;

            if (mode == 2 || mode == 3)
            {
                lineMaterial.SetFloat("_Animated", 1);
                lineMaterial.EnableKeyword("_ANIMATED_CENTRAL");
            }

            if (mode == 6)
            {
                lineMaterial.SetFloat("_Animated", 2);
                lineMaterial.EnableKeyword("_ANIMATED_DIAGONAL_CORNER");
            }

            if (mode == 4 || mode == 5)
            {
                lineMaterial.SetFloat("_Animated", 3);
                lineMaterial.EnableKeyword("_ANIMATED_DIAGONAL_CENTRE");
            }
            float t = val? 1 : 0;
            if (hDimensionMesh) hDimensionMesh.GetComponent<Renderer>().enabled = val;
            if (dDimensionMesh) dDimensionMesh.GetComponent<Renderer>().enabled = val;
            if (wDimensionMesh) wDimensionMesh.GetComponent<Renderer>().enabled = val;
            if (mode == 2 || mode == 6 || mode == 4) lineMaterial.SetFloat("_offset", t);
            if (mode == 3 || mode == 5) lineMaterial.SetFloat("_offset", 1 - t);
            if (!val) enabled = true;
            bool dimensionsSwitched = !val;
            while (val ? (t > 0) : (t < 1))
            {
                if (mode > 1 && (dimensionsSwitched != t > 0.5f))
                {
                    if (hDimensionMesh) hDimensionMesh.GetComponent<Renderer>().enabled = t > 0.5f;
                    if (dDimensionMesh) dDimensionMesh.GetComponent<Renderer>().enabled = t > 0.5f;
                    if (wDimensionMesh) wDimensionMesh.GetComponent<Renderer>().enabled = t > 0.5f;
                    dimensionsSwitched = t > 0.5f;
                }

                if ((mode == 1) && (dimensionsSwitched != (t >= 1.0f)))
                {
                    if (hDimensionMesh) hDimensionMesh.GetComponent<Renderer>().enabled = t >= 1.0f;
                    if (dDimensionMesh) dDimensionMesh.GetComponent<Renderer>().enabled = t >= 1.0f;
                    if (wDimensionMesh) wDimensionMesh.GetComponent<Renderer>().enabled = t >= 1.0f;
                    dimensionsSwitched = t >= 1.0f;
                }
                t += (val ? -1 : 1) * speed;
                if (mode == 1)
                {
                    boxProgress = t;
                    SetLines();
                }
                if (mode == 2 || mode == 6 || mode == 4) lineMaterial.SetFloat("_offset", t);
                if (mode == 3 || mode == 5) lineMaterial.SetFloat("_offset", 1 - t);
                /*if (mode > 1)
                {
                    if (hDimensionMesh) hDimensionMesh.GetComponent<Renderer>().enabled = t > 0.5f;
                    if (dDimensionMesh) dDimensionMesh.GetComponent<Renderer>().enabled = t > 0.5f;
                    if (wDimensionMesh) wDimensionMesh.GetComponent<Renderer>().enabled = t > 0.5f;
                }*/
                
                yield return new WaitForEndOfFrame();
            }
            if (hDimensionMesh) hDimensionMesh.GetComponent<Renderer>().enabled = !val;
            if (dDimensionMesh) dDimensionMesh.GetComponent<Renderer>().enabled = !val;
            if (wDimensionMesh) wDimensionMesh.GetComponent<Renderer>().enabled = !val;
            if (val) enabled = false;
            lineMaterial.SetFloat("_Animated", 0);
            lineMaterial.DisableKeyword("_ANIMATED_CENTRAL");
            lineMaterial.DisableKeyword("_ANIMATED_DIAGONAL_CORNER");
            lineMaterial.DisableKeyword("_ANIMATED_DIAGONAL_CENTRE");
            coroutineRunning = false;
            yield return null;
        }

        struct PointData
        {
            public int index;
            public Vector3 vector;
            public PointData(int _index, Vector3 _vector)
            {
                index = _index;
                vector = _vector;
            }
        }

        int SortCornerForDiagonalAnimation()
        {
            Quaternion camRot = Camera.main.transform.rotation;
            Vector3 angles = camRot.eulerAngles;
            angles.z = 0;
            Camera.main.transform.rotation = Quaternion.Euler(angles);

            List<PointData> PointsList = new List<PointData>();

            for (int i = 0; i < corners.Length; i++)
            {
                PointsList.Add(new PointData(i, Camera.main.WorldToScreenPoint(transform.TransformPoint(corners[i]))));
            }

            var result = PointsList.OrderBy(pd => pd.vector.y).ToList();
            int selectedIndex = result[0].index;
            int selectedIndex2 = result[1].index;
            int corner = selectedIndex;

            if (result[1].vector.x < result[0].vector.x)
            {
                corner = selectedIndex2;
            }
            Camera.main.transform.rotation = camRot;
            return corner;
        }

        void DoEndPoints()
        {
            for (int i = 0; i < 4; i++)
            {
                float _scale = i % 2 * bound.size.x + (i + 1) % 2 * bound.size.z;
                //bottom rect
                animationEndPoints[i] = corners[i + 4] - Quaternion.AngleAxis(-90 * i, Vector3.up) * Vector3.forward * _scale * boxProgress;
                //height
                animationEndPoints[i + 4] = i % 2 * corners[i + 4] + (i + 1) % 2 * corners[i] - Vector3.up * bound.size.y * (1 - 2 * (i % 2)) * boxProgress;
                //top rect
                animationEndPoints[i + 8] = corners[i] - Quaternion.AngleAxis(-90 * i, Vector3.up) * Vector3.forward * _scale * boxProgress;
            }
        }

        float GetTextHeight()
        {
            return 0.1f*charSize;
        }

    }
}
