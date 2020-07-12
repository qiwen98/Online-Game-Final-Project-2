using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("Rules Panel")]
    public GameObject RulesUIPanel;

    [Header("GameOptions Panel")]
    public GameObject CreateOrJoinUIPanel;

    public GameObject GeneralRulePanel;
    public GameObject TrapUIPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToRulePage()
    {
        CreateOrJoinUIPanel.SetActive(false);
        GeneralRulePanel.SetActive(false);
        TrapUIPanel.SetActive(false);
        RulesUIPanel.SetActive(true);
    }

    public void GoToCreateOrJoin()
    {
        RulesUIPanel.SetActive(false);
        CreateOrJoinUIPanel.SetActive(true);
    }

    public void GoToGeneralRule()
    {
        GeneralRulePanel.SetActive(true);
    }

    public void GoToTrap()
    {
        TrapUIPanel.SetActive(true);
    }
}
