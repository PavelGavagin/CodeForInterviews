using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Game : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textAmountLeft; // доступ к тексту который отображает сколько монет оталось на столе
    public int amountLeft = 300; // переменная количества монет которые лежат на столе.
    public int[]AllWalletI = new int[9]; // Массив всеж кошельков.
    public int sumAllWalet = 0; // переменная отвечающая за сумму денег в кошельках
    public bool NegativeMoneyTriger = false; // тригер показывающий что монеты закончились на столе.
    private Coroutine resetCoroutine;//Куратина для сброса триггера, когда минусовой балланс на столе
    public bool allNonZero = true;// Тригер что в каждом кошельке есть манеты
    public GameObject VictoryPanel;
    public GameObject PanelError; // Ссылка на панель ошибки
    public bool allValid = true; // Флаг для проверки, все ли числа допустимы
    public TMP_InputField[] inputFields; // Массив InputField (TMP)
    private List<int> validNumbers = new List<int> { 1, 2, 4, 8, 16, 32, 64, 128, 45 }; // Список допустимых чисел


   void Start(){
        VictoryPanel.SetActive(false);// Скрываем панель победы при старте
        PanelError.SetActive(false); // Скрываем панель неправельного ответа при старте
        allNonZero = true; // Тригер на проверку манет
   }         
    void Update()
        {
            CoinsOnTable(); //отображение количества монет на столе
        }

    void CoinsOnTable() //отображение количества монет на столе
        {
            textAmountLeft.text = amountLeft.ToString();
        }

    public void SumManyWallit() // Сложение всех чисел в массиве
        {
            sumAllWalet = 0; // переменная отвечающая за сумму денег в кошельках
                for (int i = 0; i < AllWalletI.Length; i++)
                    {
                        sumAllWalet += AllWalletI[i]; // Добавляем текущий элемент к сумме
                    }

                ThatNoMinus (); // Считает сколько осталось манет на столе
        }
    void ThatNoMinus () // Считает сколько осталось манет на столе, чтобы небыло минуса
            {
                amountLeft = 300 - sumAllWalet;
                if (amountLeft<0)
                    {
                        NegativeMoneyTriger = true;
                    }

                CoinsOnTable(); //отображение количества монет на столе
            }
    void OnGUI()// Когда денги на столе заканчиваются, выводится сообщение СТОЛ ПУСТ
        {
               if (NegativeMoneyTriger) // Проверяем состояние триггера
                {
                    // Создаем текстуру желтого цвета
                    Texture2D yellowTexture = new Texture2D(1, 1);
                    yellowTexture.SetPixel(0, 0, Color.black);
                    yellowTexture.Apply();

                    // Задаем размеры и позицию фона
                    float width = 800;
                    float height = 500;
                    float x = (Screen.width - width) / 2; // Центрирование по горизонтали
                    float y = (Screen.height - height) / 2; // Центрирование по вертикали

                    // Рисуем фон
                    GUI.DrawTexture(new Rect(x, y, width, height), yellowTexture);

                    // Создаем стиль для текста
                    GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                    labelStyle.fontSize = 75;           // Устанавливаем размер шрифта
                    labelStyle.normal.textColor = Color.white; // Устанавливаем цвет текста
                    labelStyle.alignment = TextAnchor.MiddleCenter; // Выравнивание по центру

                    // Отображаем сообщение с использованием стиля
                    GUI.Label(new Rect(x, y, width, height), "Ай - ай - ай! Монеты на столе закончились.", labelStyle);

                    // Запускаем корутину для сброса триггера
                    if (resetCoroutine == null)
                        {
                            resetCoroutine = StartCoroutine(ResetTriggerAfterDelay());
                        }
                }
        }

                private IEnumerator ResetTriggerAfterDelay()
                    {
                        yield return new WaitForSeconds(2); // Ждем 2 секунды
                        NegativeMoneyTriger = false; // Сбрасываем триггер
                        resetCoroutine = null; // Сбрасываем корутину
                    }

     public void CheckInputFields() // Проверяем на правельность введеных чисел в InputFields 
    {
        // Сбрасываем флаг перед проверкой
        allValid = true;

        // Проходим по всем InputField
        for (int i = 0; i < inputFields.Length; i++)
        {
            // Получаем текст из InputField
            string inputText = inputFields[i].text;

            // Пытаемся преобразовать текст в число
            if (int.TryParse(inputText, out int number))
            {
                // Проверяем, есть ли число в списке допустимых
                if (!validNumbers.Contains(number))
                {
                    Debug.Log($"Ошибка: в поле {i + 1} введено недопустимое число: {number}");
                    allValid = false;
                }
            }
            else
            {
                Debug.Log($"Ошибка: в поле {i + 1} введён нечисловой текст: {inputText}");
                allValid = false;
            }
        }

        // Если все числа допустимы
        if (allValid)
        {
            Debug.Log("Все числа корректны!");
            VictoryPanel.SetActive(true); // ввыводим окно победы
        }
        else
        {
            Debug.Log("Обнаружены ошибки в введённых числах.");
            ShowError();
            
        }
    }


public void ClickIfReady ()
        {
           
            CheckInputFields();
        }

public void ShowError()
    {
        // Показываем панель
        PanelError.SetActive(true);
        
        // Запускаем корутину для скрытия через 3 секунды
        StartCoroutine(HideAfterDelay(3f));
    }

    private System.Collections.IEnumerator HideAfterDelay(float delay)
    {
        // Ждем указанное количество секунд
        yield return new WaitForSeconds(delay);
        
        // Скрываем панель
        PanelError.SetActive(false);
    }
}





    
    