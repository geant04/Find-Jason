using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject inGameDisplay;
    public VisualTreeAsset itemLabel;

    private UIDocument uiDocument;
    private PopUpBox centerBox; // used for text like "... shit got activated"
    private PopUpBox infoBox; // used for text like "Press E to activate button"
    private PopUpBox sideBox; // used for text like "+10 ammo picked up, fade..."
    private GroupBox data;
    private Label hpDisplay;
    private Label ammoDisplay;

    public void Awake()
    {
        if (!inGameDisplay) return;
        uiDocument = inGameDisplay.GetComponent<UIDocument>();

        if (!uiDocument) return;
        centerBox = new PopUpBox(
            uiDocument.rootVisualElement.Q("CenterBox") as GroupBox, 
            itemLabel, 
            this);

        infoBox = new PopUpBox(
            uiDocument.rootVisualElement.Q("InfoBox") as GroupBox,
            itemLabel,
            this);

        data = uiDocument.rootVisualElement.Q("Data") as GroupBox;
        hpDisplay = data.Q("HP") as Label;
        ammoDisplay = data.Q("Ammo") as Label;
    }

    public void SetAmmo(string text)
    {
        ammoDisplay.text = text;
    }

    public void SetHP(string text)
    {
        hpDisplay.text = text;
    }

    public void InsertToCenterBox(MessageUI message)
    {
        centerBox.InsertPopUp(message);
    }

    public void ShowInfo(string text, float lifeTime)
    {
        if (infoBox.popUps.Count <= 0)
        {
            MessageUI message;
            message.text = text;
            message.lifeTime = lifeTime;
            infoBox.InsertPopUp(message);
        }

        infoBox.popUps[0].lifeTime = lifeTime;
        infoBox.popUps[0].text = text;
    }

    public void InsertToCenterBox(string text, float lifeTime)
    {
        MessageUI message;
        message.text = text;
        message.lifeTime = lifeTime;
        centerBox.InsertPopUp(message);
    }
}

public struct MessageUI // by default will fade
{
    public string text;
    public float lifeTime;
}

public class PopUpBox
{
    public List<UIPopUp> popUps = new List<UIPopUp>();
    public VisualTreeAsset itemLabel;
    public PlayerUIManager UImanager;
    public GroupBox groupBox;

    public PopUpBox(GroupBox groupBox, VisualTreeAsset itemLabel, PlayerUIManager UImanager)
    {
        this.groupBox = groupBox;
        this.itemLabel = itemLabel;
        this.UImanager = UImanager;
    }

    public void InsertPopUp(MessageUI message)
    {
        UIPopUp popUp = new UIPopUp(message.text, message.lifeTime, itemLabel, this);
        popUps.Insert(0, popUp);
    }
    public void RemovePopUp(UIPopUp item, TemplateContainer label)
    {
        groupBox.Remove(label);
        popUps.Remove(item);
    }
}

public class UIPopUp
{
    public float lifeTime;
    public string text;
    private TemplateContainer root;
    private PopUpBox parent;

    public UIPopUp(string text, float lifeTime, VisualTreeAsset labelAsset, PopUpBox parent)
    {
        this.text = text;
        this.lifeTime = lifeTime;
        this.parent = parent;

        root = labelAsset.Instantiate();
        parent.groupBox.Add(root);
        parent.UImanager.StartCoroutine(ShowText());
    }

    public IEnumerator ShowText()
    {
        Label label = root.Q<Label>("Msg");
        label.text = text;

        while (this.lifeTime > 0) // if we change liftTime from outside, use class's member var
        {
            this.lifeTime -= Time.deltaTime;
            yield return null;
        }

        parent.RemovePopUp(this, root);
        yield return null;
    }
}

