using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WorkerInfo
{
    public string workerName;
    public WorkerJob workerJob;
    public float workerExertionAmount = 1;
    public float workerPoints = 1;
    public float workerPercentage = 100;
    public float workerPay = 100;

    public WorkerInfo(WorkerJob _workerJob)
    {
        workerJob = _workerJob;
    }
}

[CreateAssetMenu(fileName = "New Worker Job", menuName = "WorkerJob")]
public class WorkerJob : ScriptableObject
{
    public string workName;
    public int workPoints;
    [HideInInspector] public int workNumber;
}

public class WorkersPayCalculator : MonoBehaviour
{
    #region Fields
    [Header("Info")]
    [SerializeField] List<WorkerJob> workerJobTypes = new List<WorkerJob>();
    [Tooltip("Minimum Number Of Worker Name's Letters")] [Range(1, 10)] [SerializeField] int minimumNumberOfLetters = 1;

    [Header("References")]
    [SerializeField] Transform EnterTeamEarning;
    TMP_InputField teamEarningInputField;
    Button startButton;

    [SerializeField] Transform fillingTheInfo;
    TextMeshProUGUI workersNumberText;
    TextMeshProUGUI currentWorkerNumberText;
    Transform info;
    TMP_InputField workerNameInputField;
    TMP_Dropdown workerJobDropdown;
    Slider workerExertionSlider;
    TextMeshProUGUI workerExertionText;

    Transform buttons;
    Button newWorkerButton;
    Button removeWorkerButton;
    Button nextWorkerButton;
    Button previousWorkerButton;
    Button showResultsButton;
    [SerializeField] Transform results;
    [SerializeField] GameObject resultColumn;

    float teamEarning = 1;

    List<WorkerInfo> workersInfo = new List<WorkerInfo>();
    int currentWorkerNumber = -1;
    #endregion

    void Start()
    {
        #region References
        workersNumberText = fillingTheInfo.GetChild(0).GetComponent<TextMeshProUGUI>();
        currentWorkerNumberText = fillingTheInfo.GetChild(1).GetComponent<TextMeshProUGUI>();
        info = fillingTheInfo.GetChild(2);
        buttons = fillingTheInfo.GetChild(3);

        teamEarningInputField = EnterTeamEarning.GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
        startButton = EnterTeamEarning.GetChild(1).GetComponent<Button>();

        workerNameInputField = info.GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
        workerJobDropdown = info.GetChild(1).GetChild(0).GetComponent<TMP_Dropdown>();
        workerExertionSlider = info.GetChild(2).GetChild(0).GetComponent<Slider>();
        workerExertionText = info.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();

        newWorkerButton = buttons.GetChild(0).GetComponent<Button>();
        removeWorkerButton = buttons.GetChild(1).GetComponent<Button>();
        nextWorkerButton = buttons.GetChild(2).GetComponent<Button>();
        previousWorkerButton = buttons.GetChild(3).GetComponent<Button>();
        showResultsButton = buttons.GetChild(4).GetComponent<Button>();
        #endregion

        #region Add WorkerJobs Types to the Dropdown
        workerJobDropdown.options.Clear();
        for (int i = 0; i < workerJobTypes.Count; i++)
        {
            workerJobTypes[i].workNumber = i;
            if (CheckStringIsNull(workerJobTypes[i].workName))
                workerJobDropdown.options.Add(new TMP_Dropdown.OptionData { text = "Test" + (i + 1) });
            else
                workerJobDropdown.options.Add(new TMP_Dropdown.OptionData { text = workerJobTypes[i].workName });
        }
        #endregion

        #region Default Buttons State
        ChangeButtonState(startButton, false);
        ChangeButtonState(showResultsButton, false);
        ChangeButtonState(removeWorkerButton, false);
        ChangeButtonState(previousWorkerButton, false);

        nextWorkerButton.gameObject.SetActive(false);
        showResultsButton.gameObject.SetActive(false);
        #endregion

        NewWorker();
    }

    public void OnChangeTeamEarning(string teamEarning) => ChangeButtonState(startButton, CheckStringIsNull(teamEarning) || float.Parse(teamEarning) <= 0);

    public void StartFillingTheInfo()
    {
        EnterTeamEarning.gameObject.SetActive(false);
        fillingTheInfo.gameObject.SetActive(true);

        teamEarning = float.Parse(teamEarningInputField.text);
    }

    public void OnChangeWorkerName(string workerName)
    {
        bool isOn = workerName.Length >= minimumNumberOfLetters;

        ChangeButtonState(newWorkerButton.gameObject.activeInHierarchy ? newWorkerButton : nextWorkerButton, isOn);
        ChangeButtonState(previousWorkerButton, isOn);
        ChangeButtonState(showResultsButton, isOn);

        workersInfo[currentWorkerNumber].workerName = workerName;
    }

    public void OnChangeWorkerJob(int workerJobIndex) => workersInfo[currentWorkerNumber].workerJob = workerJobTypes[workerJobIndex];

    public void OnChangeWorkerExertion(float workerExertionAmount)
    {
        if (workerExertionAmount % 2 != 0)
            workerExertionAmount--;

        workerExertionText.text = string.Format($"Exertion Amount : {workerExertionAmount * 0.1f}");
        workersInfo[currentWorkerNumber].workerExertionAmount = workerExertionAmount * 0.1f;
    }

    void ChangeButtonState(Button button, bool isOn)
    {
        button.interactable = isOn;
        button.GetComponent<Image>().color = isOn ? Color.white : Color.grey;
    }

    bool CheckStringIsNull(string text) => string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);

    public void NewWorker()
    {
        workersInfo.Add(new WorkerInfo(workerJobTypes[0]));
        currentWorkerNumber++;

        RefreshInfo(new WorkerInfo(workerJobTypes[0]));
    }

    public void RemoveWorker()
    {
        workersInfo.Remove(workersInfo[currentWorkerNumber]);
        currentWorkerNumber--;

        RefreshInfo(workersInfo[currentWorkerNumber]);
    }

    public void PreviousWorker()
    {
        currentWorkerNumber--;
        RefreshInfo(workersInfo[currentWorkerNumber]);
    }

    public void NextWorker()
    {
        currentWorkerNumber++;
        RefreshInfo(workersInfo[currentWorkerNumber]);
    }

    void RefreshInfo(WorkerInfo workerInfo)
    {
        workersNumberText.text = string.Format($"Workers Number : {workersInfo.Count}");
        currentWorkerNumberText.text = string.Format($"Current Worker Number : {currentWorkerNumber + 1}");

        workerNameInputField.text = workerInfo.workerName;
        workerJobDropdown.value = workerInfo.workerJob.workNumber;
        workerExertionSlider.value = workerInfo.workerExertionAmount * 10;
        workerExertionText.text = $"{workerInfo.workerExertionAmount}";

        bool sm1 = currentWorkerNumber == 0;
        ChangeButtonState(removeWorkerButton, !sm1);
        previousWorkerButton.gameObject.SetActive(!sm1);

        bool sm2 = workersInfo.Count == currentWorkerNumber + 1;
        newWorkerButton.gameObject.SetActive(sm2);
        nextWorkerButton.gameObject.SetActive(!sm2);
        showResultsButton.gameObject.SetActive(sm2);
    }

    public void ShowResults()
    {
        fillingTheInfo.gameObject.SetActive(false);
        results.gameObject.SetActive(true);

        float workersTotalPoints = 0;
        for (int i = 0; i < workersInfo.Count; i++)
        {
            workersInfo[i].workerPoints = workersInfo[i].workerJob.workPoints * workersInfo[i].workerExertionAmount;
            workersTotalPoints += workersInfo[i].workerPoints;
        }

        for (int i = 0; i < workersInfo.Count; i++)
        {
            workersInfo[i].workerPercentage = workersInfo[i].workerPoints / workersTotalPoints;
            workersInfo[i].workerPay = workersInfo[i].workerPercentage * teamEarning;

            Transform _resultColumn = Instantiate(resultColumn, resultColumn.transform.parent).transform;
            _resultColumn.GetChild(0).GetComponent<TextMeshProUGUI>().text = workersInfo[i].workerName;
            _resultColumn.GetChild(1).GetComponent<TextMeshProUGUI>().text = workersInfo[i].workerJob.workName;
            _resultColumn.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{workersInfo[i].workerPay}$";
            _resultColumn.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"{workersInfo[i].workerPercentage * 100}%";
        }

        resultColumn.SetActive(false);
    }
}