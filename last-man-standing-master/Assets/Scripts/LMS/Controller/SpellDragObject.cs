using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LMS.Battle
{
    public class SpellDragObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static GameObject itemBeingDragged;
        public delegate void OnFinishDrag(Vector2 point);
        public OnFinishDrag CallBackOnFinishDrag;

        public float ScaleAOE = 1;
        [SerializeField]
        private Transform spellAOE;

        private Vector3 startPosition;
        private Transform startParent;
        private const float MinY = 120;
        private void Start()
        {
            spellAOE.gameObject.SetActive(false);
        }

        public void SetEnableDragable(bool enable)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = enable;
        }

        #region IDragHandler implementation

        public void OnBeginDrag(PointerEventData eventData)
        {
            itemBeingDragged = gameObject;
            startPosition = transform.position;
            startParent = transform.parent;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            transform.SetParent(BattleManager.Instance.DropSkillParent);
            //Debug.Log(startPosition);
            spellAOE.gameObject.SetActive(true);
            spellAOE.transform.localScale = new Vector3(ScaleAOE, ScaleAOE);
        }
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
            //Debug.Log("Event: " + eventData.position);
            if (eventData.position.y <= MinY) return;
            // Raycast AOE spell
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green, 1);
            int layer = LayerMask.GetMask("Background");
            if (Physics.Raycast(ray, out hit, 1000, layer))
            {
                spellAOE.transform.position = hit.point;
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            //Debug.Log("OnEnd");
            itemBeingDragged = null;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            //Debug.Log(transform.position);
            //if (transform.parent == startParent)
            //{
            //    transform.position = startPosition;
            //}
            //else if (transform.parent == BattleManager.Instance.DropSkillParent)
            //{
            //    transform.SetParent(startParent);
            //    transform.position = startPosition;
            //}
            transform.SetParent(startParent);
            transform.position = startPosition;

            spellAOE.gameObject.SetActive(false);
            if (eventData.position.y <= MinY) return;
            // Raycast AOE spell
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green, 1);
            int layer = LayerMask.GetMask("Background");
            if (Physics.Raycast(ray, out hit, 1000, layer))
            {
                spellAOE.transform.position = hit.point;
                CallBackOnFinishDrag(hit.point);
            }
        }

        #endregion
#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(spellAOE.transform.position, spellAOE.transform.forward, ScaleAOE / 2);
        }
#endif
    }
}