using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Model;

namespace LMS.Battle
{
    public class PowerUpObject : MonoBehaviour
    {

        private PowerUpScript source;

        public void LoadPowerUp(PowerUpScript source)
        {
            this.source = source;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //Debug.Log("Enter");
            //Debug.Log(col.gameObject.layer); 
            //Debug.Log(LayerMask.NameToLayer(Common.layerUnit));
            if (col.gameObject.layer == LayerMask.NameToLayer(CONSTANT.layerUnit))
            {
                // Only character - unit
                source.OnTriggerPowerUp(col.gameObject.GetComponent<UnitAnimation>().unitObject);
                if (col.gameObject.GetComponent<UnitAnimation>().unitObject.CanTakePowerUp)
                {
                    GameObject obj = Instantiate(Resources.Load(string.Format(CONSTANT.PathPowerUp, CONSTANT.VFXTakePowerUp)) as GameObject);
                    obj.transform.SetParent(col.transform);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    obj.transform.localPosition = Vector3.zero;
                    DestroyItSelf();
                }
            }
        }

        private void DestroyItSelf()
        {
            Destroy(gameObject);
        }
    }
}