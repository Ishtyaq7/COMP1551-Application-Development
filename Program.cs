using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        QuizGame game = new QuizGame();
        game.Start();
    }
}

class QuizGame
{
    List<Question> questions;

    public QuizGame()
    {
        questions = new List<Question>();
    }

    public void Start()
    {
        while (true)
        {
            Console.WriteLine("1. Create a New Game");
            Console.WriteLine("2. Edit Existing Game");
            Console.WriteLine("3. Play");
            Console.WriteLine("4. Exit");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    CreateNewGame();
                    break;
                case "2":
                    EditExistingGame();
                    break;
                case "3":
                    PlayGame();
                    break;
                case "4":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private void CreateNewGame()
    {
        Console.WriteLine("Create a New Game Mode");

        // Clear the list of questions
        questions.Clear();

        // Allow the user to add questions
        Console.WriteLine("Add questions:");
        bool addingQuestions = true;
        while (addingQuestions)
        {
            Console.WriteLine("Enter the type of question (MC for multiple choice, OE for open-ended, TF for true/false, or QUIT to stop adding questions):");
            string questionType = Console.ReadLine().ToUpper();

            if (questionType == "QUIT")
            {
                addingQuestions = false;
            }
            else
            {
                Question newQuestion = null;

                switch (questionType)
                {
                    case "MC":
                        newQuestion = AddMultipleChoiceQuestion();
                        break;
                    case "OE":
                        newQuestion = AddOpenEndedQuestion();
                        break;
                    case "TF":
                        newQuestion = AddTrueFalseQuestion();
                        break;
                    default:
                        Console.WriteLine("Invalid question type.");
                        break;
                }

                if (newQuestion != null)
                {
                    questions.Add(newQuestion);
                    Console.WriteLine("Question added successfully.");
                }
            }
        }
    }

    private void EditExistingGame()
    {
        if (questions.Count == 0)
        {
            Console.WriteLine("There's no existing game to edit.");
            return;
        }

        Console.WriteLine("Edit Existing Game Mode");

        // Allow the user to add or remove questions
        Console.WriteLine("Add or remove questions:");
        bool editingQuestions = true;
        while (editingQuestions)
        {
            Console.WriteLine("Enter the type of action (ADD to add a question, REMOVE to remove a question, or QUIT to finish editing):");
            string action = Console.ReadLine().ToUpper();

            switch (action)
            {
                case "ADD":
                    Question newQuestion = AddQuestion();
                    if (newQuestion != null)
                    {
                        questions.Add(newQuestion);
                        Console.WriteLine("Question added successfully.");
                    }
                    break;
                case "REMOVE":
                    RemoveQuestion();
                    break;
                case "QUIT":
                    editingQuestions = false;
                    break;
                default:
                    Console.WriteLine("Invalid action. Please try again.");
                    break;
            }
        }
    }

    private Question AddQuestion()
    {
        Console.WriteLine("Enter the type of question (MC for multiple choice, OE for open-ended, TF for true/false):");
        string questionType = Console.ReadLine().ToUpper();

        switch (questionType)
        {
            case "MC":
                return AddMultipleChoiceQuestion();
            case "OE":
                return AddOpenEndedQuestion();
            case "TF":
                return AddTrueFalseQuestion();
            default:
                Console.WriteLine("Invalid question type.");
                return null;
        }
    }

    private void RemoveQuestion()
    {
        if (questions.Count == 0)
        {
            Console.WriteLine("There are no questions to remove.");
            return;
        }

        Console.WriteLine("Select the index of the question to remove:");

        for (int i = 0; i < questions.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {questions[i].Text}");
        }

        int selectedIndex;
        if (int.TryParse(Console.ReadLine(), out selectedIndex) && selectedIndex >= 1 && selectedIndex <= questions.Count)
        {
            questions.RemoveAt(selectedIndex - 1);
            Console.WriteLine("Question removed successfully.");
        }
        else
        {
            Console.WriteLine("Invalid index.");
        }
    }

    private MultipleChoiceQuestion AddMultipleChoiceQuestion()
    {
        Console.WriteLine("Enter the text of the multiple choice question:");
        string questionText = Console.ReadLine();

        Console.WriteLine("Enter the choices (separated by commas):");
        string choicesInput = Console.ReadLine();
        string[] choices = choicesInput.Split(',');

        Console.WriteLine("Enter the index of the correct choice:");
        int correctIndex = Convert.ToInt32(Console.ReadLine());

        MultipleChoiceQuestion question = new MultipleChoiceQuestion
        {
            Text = questionText,
            Choices = choices.ToList(),
            CorrectChoiceIndex = correctIndex
        };

        return question;
    }

    private OpenEndedQuestion AddOpenEndedQuestion()
    {
        Console.WriteLine("Enter the text of the open-ended question:");
        string questionText = Console.ReadLine();

        Console.WriteLine("Enter the correct answer:");
        string correctAnswer = Console.ReadLine();

        OpenEndedQuestion question = new OpenEndedQuestion
        {
            Text = questionText,
            CorrectAnswer = correctAnswer
        };

        return question;
    }

    private TrueFalseQuestion AddTrueFalseQuestion()
    {
        Console.WriteLine("Enter the text of the true/false question:");
        string questionText = Console.ReadLine();

        Console.WriteLine("Enter the correct answer (True or False):");
        string correctAnswer = Console.ReadLine();

        TrueFalseQuestion question = new TrueFalseQuestion
        {
            Text = questionText,
            CorrectAnswer = correctAnswer.ToLower() == "true" ? true : false
        };

        return question;
    }

    private void PlayGame()
    {
        Console.WriteLine("Play Mode");

        int score = 0;
        DateTime startTime = DateTime.Now;

        foreach (Question question in questions)
        {
            Console.WriteLine(question.Text);

            if (question is MultipleChoiceQuestion)
            {
                DisplayMultipleChoiceOptions(((MultipleChoiceQuestion)question).Choices);
            }

            Console.WriteLine("Enter your answer:");
            string userAnswer = Console.ReadLine();

            if (question.CheckAnswer(userAnswer))
            {
                Console.WriteLine("Correct!");
                score++;
            }
            else
            {
                Console.WriteLine("Incorrect!");
            }
        }

        TimeSpan elapsedTime = DateTime.Now - startTime;
        double minutes = elapsedTime.TotalMinutes;

        Console.WriteLine($"Your score: {score}/{questions.Count}");
        Console.WriteLine($"Time taken: {minutes} minutes");

        // Ask to view correct answers after finishing the game
        Console.WriteLine("Do you want to view the correct answers? (Y/N)");
        string viewAnswersOption = Console.ReadLine().ToUpper();
        if (viewAnswersOption == "Y")
        {
            ViewCorrectAnswers();
        }
    }

    private void DisplayMultipleChoiceOptions(List<string> choices)
    {
        for (int i = 0; i < choices.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {choices[i]}");
        }
    }

    private void ViewCorrectAnswers()
    {
        if (questions.Count == 0)
        {
            Console.WriteLine("There's no game to view answers for. Create a new game first.");
            return;
        }

        Console.WriteLine("Correct Answers:");

        foreach (Question question in questions)
        {
            Console.WriteLine($"{question.Text}: {GetCorrectAnswer(question)}");
        }
    }

    private string GetCorrectAnswer(Question question)
    {
        if (question is MultipleChoiceQuestion)
        {
            return ((MultipleChoiceQuestion)question).Choices[((MultipleChoiceQuestion)question).CorrectChoiceIndex];
        }
        else if (question is OpenEndedQuestion)
        {
            return ((OpenEndedQuestion)question).CorrectAnswer;
        }
        else if (question is TrueFalseQuestion)
        {
            return ((TrueFalseQuestion)question).CorrectAnswer ? "True" : "False";
        }
        else
        {
            return "N/A";
        }
    }
}

public abstract class Question
{
    public string Text { get; set; }
    public abstract bool CheckAnswer(string answer);
}

public class MultipleChoiceQuestion : Question
{
    public List<string> Choices { get; set; }
    public int CorrectChoiceIndex { get; set; }

    public override bool CheckAnswer(string answer)
    {
        int userChoice;
        if (int.TryParse(answer, out userChoice))
        {
            return userChoice == CorrectChoiceIndex;
        }
        return false;
    }
}

public class OpenEndedQuestion : Question
{
    public string CorrectAnswer { get; set; }

    public override bool CheckAnswer(string answer)
    {
        return string.Equals(answer, CorrectAnswer, StringComparison.OrdinalIgnoreCase);
    }
}

public class TrueFalseQuestion : Question
{
    public bool CorrectAnswer { get; set; }

    public override bool CheckAnswer(string answer)
    {
        bool userAnswer;
        if (bool.TryParse(answer, out userAnswer))
        {
            return userAnswer == CorrectAnswer;
        }
        return false;
    }
}
