using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vts.mvc;

namespace vts
{
    public class MainActivity : MonoBehaviour
    {
        [Header("Fragment")]
        #region Fragment
        public GameObject bookmark_fragment;
        public GameObject speech_fragment;
        #endregion

        [Header("Activity")]
        public GameObject speech_activity;

        // Start is called before the first frame update
        void Start()
        {
            if (!AppFacade.getInstance().hasMediator(MediatorName.MainActivity))
            {
                MainActivityMediator mediator = new MainActivityMediator(mediator_name: MediatorName.MainActivity, activity: gameObject);
                AppFacade.getInstance().registerMediator(mediator);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                speech_activity.SetActive(true);
                gameObject.SetActive(false);
            }

            //if (Input.GetMouseButtonDown(0))
            //{
            //    Utils.log(Input.mousePosition.ToString());
            //}
        }
    }
}
