using System;
using System.Windows;
using System.Windows.Controls;

namespace LB1_LOBKOVA
{
    public partial class MainWindow : Window
    {
        private double firstNumber = 0;           
        private string currentOperation = "";     
        private bool isNewEntry = true;           
        private bool isDegrees = true;            // Флаг для режима углов (градусы или радианы)
        private string expression = "";           // Хранит текущее выражение для отображения

        public MainWindow()
        {
            InitializeComponent();
            StatusBarText.Text = $"Время запуска: {DateTime.Now:HH:mm:ss}";
            InputBox.Focus(); // Устанавливаем начальный фокус на поле ввода
        }

        // Обработчик для кнопок с цифрами и точкой
        private void Digit_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (isNewEntry)
                {
                    InputBox.Text = "";
                    isNewEntry = false;
                }
                InputBox.Text += button.Content.ToString();
                InputBox.CaretIndex = InputBox.Text.Length;
            }
        }

        // Обработчики для операций (+, -, *, /)
        private void Add_Click(object sender, RoutedEventArgs e) => SetOperation("+");
        private void Subtract_Click(object sender, RoutedEventArgs e) => SetOperation("-");
        private void Multiply_Click(object sender, RoutedEventArgs e) => SetOperation("*");
        private void Divide_Click(object sender, RoutedEventArgs e) => SetOperation("/");

        private void SetOperation(string operation)
        {
            if (double.TryParse(InputBox.Text, out firstNumber))
            {
                currentOperation = operation;
                expression = $"{firstNumber} {operation}";
                InputBox.Text = expression;
                isNewEntry = true;
                InputBox.Focus();
                InputBox.CaretIndex = InputBox.Text.Length;
            }
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(InputBox.Text.Substring(expression.Length).Trim(), out double secondNumber))
            {
                double result = 0;
                switch (currentOperation)
                {
                    case "+":
                        result = firstNumber + secondNumber;
                        break;
                    case "-":
                        result = firstNumber - secondNumber;
                        break;
                    case "*":
                        result = firstNumber * secondNumber;
                        break;
                    case "/":
                        if (secondNumber == 0)
                        {
                            ShowError("Деление на ноль невозможно");
                            return;
                        }
                        result = firstNumber / secondNumber;
                        break;
                    case "^":
                        result = Math.Pow(firstNumber, secondNumber);
                        break;
                }

                expression += $" {secondNumber} = {result}";
                InputBox.Text = expression;
                HistoryBox.Items.Insert(0, expression);

                firstNumber = result;
                isNewEntry = true;
                currentOperation = "";
                expression = result.ToString();
                InputBox.Focus();
                InputBox.CaretIndex = InputBox.Text.Length;
            }
        }

        // Унарные операции
        private void Inverse_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(InputBox.Text, out double number))
            {
                if (number == 0)
                {
                    ShowError("Обратное значение невозможно для нуля");
                    return;
                }
                double result = 1 / number;
                expression = $"1 / {number} = {result}";
                InputBox.Text = expression;
                HistoryBox.Items.Insert(0, expression);
                firstNumber = result;
                isNewEntry = true;
                InputBox.Focus();
                InputBox.CaretIndex = InputBox.Text.Length;
            }
        }

        private void SquareRoot_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(InputBox.Text, out double number))
            {
                if (number < 0)
                {
                    ShowError("Квадратный корень из отрицательного числа недопустим");
                    return;
                }
                double result = Math.Sqrt(number);
                expression = $"√{number} = {result}";
                InputBox.Text = expression;
                HistoryBox.Items.Insert(0, expression);
                firstNumber = result;
                isNewEntry = true;
                InputBox.Focus();
                InputBox.CaretIndex = InputBox.Text.Length;
            }
        }

        private void Power_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(InputBox.Text, out firstNumber))
            {
                currentOperation = "^";
                expression = $"{firstNumber} ^";
                InputBox.Text = expression;
                isNewEntry = true;
                InputBox.Focus();
                InputBox.CaretIndex = InputBox.Text.Length;
            }
        }

        // Тригонометрические функции
        private void Sin_Click(object sender, RoutedEventArgs e) => CalculateTrig(Math.Sin, "Sin");
        private void Cos_Click(object sender, RoutedEventArgs e) => CalculateTrig(Math.Cos, "Cos");
        private void Tan_Click(object sender, RoutedEventArgs e) => CalculateTrig(Math.Tan, "Tan");

        private void CalculateTrig(Func<double, double> trigFunc, string funcName)
        {
            if (double.TryParse(InputBox.Text, out double angle))
            {
                if (isDegrees)
                    angle = Math.PI * angle / 180;

                double result = trigFunc(angle);
                expression = $"{funcName}({angle}) = {result}";
                InputBox.Text = expression;
                HistoryBox.Items.Insert(0, expression);
                firstNumber = result;
                isNewEntry = true;
                InputBox.Focus();
                InputBox.CaretIndex = InputBox.Text.Length;
            }
        }

        // Переключение режима углов (градусы или радианы)
        private void AngleMode_Checked(object sender, RoutedEventArgs e)
        {
            isDegrees = (sender as RadioButton)?.Content.ToString() == "Градусы";
            InputBox.Focus();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Text = "";
            firstNumber = 0;
            currentOperation = "";
            isNewEntry = true;
            expression = "";
            InputBox.Focus();
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            InputBox.Focus();
        }
    }
}
