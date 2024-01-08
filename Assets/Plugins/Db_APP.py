import tkinter as tk
import sqlite3

def create_db():
    conn = sqlite3.connect('Db_Unity.db')
    c = conn.cursor()
    c.execute('''CREATE TABLE IF NOT EXISTS QUESTIONS (ID INTEGER PRIMARY KEY, STRING TEXT)''')
    c.execute('''CREATE TABLE IF NOT EXISTS ANSWERS (ID INTEGER PRIMARY KEY, STRING TEXT, ParentID INTEGER, IsCorrect INTEGER, FOREIGN KEY(ParentID) REFERENCES QUESTIONS(ID))''')
    conn.commit()
    conn.close()

def add_question_and_answers(question, answers, correct_answer_index):
    conn = sqlite3.connect('Db_Unity.db')
    c = conn.cursor()

    # Ajouter la question
    c.execute("INSERT INTO QUESTIONS (STRING) VALUES (?)", (question,))
    question_id = c.lastrowid

    # Ajouter les réponses
    for i, answer in enumerate(answers):
        is_correct = 1 if i == correct_answer_index else 0
        c.execute("INSERT INTO ANSWERS (STRING, ParentID, IsCorrect) VALUES (?, ?, ?)", (answer, question_id, is_correct))

    conn.commit()
    conn.close()
    print(f"Question ajoutée: '{question}' avec les réponses: {answers}. Bonne réponse: {answers[correct_answer_index]}")

def submit():
    question = entry_question.get()
    answers = [entry_answer1.get(), entry_answer2.get()]
    correct_answer_index = correct_answer_var.get()

    add_question_and_answers(question, answers, correct_answer_index)

# Création de l'interface utilisateur
create_db()
root = tk.Tk()
root.title("Quiz Manager")
root.geometry("400x200") # Taille de la fenêtre

tk.Label(root, text="Question:").grid(row=0, column=0)
entry_question = tk.Entry(root, width=50)
entry_question.grid(row=0, column=1)

tk.Label(root, text="Réponse 1:").grid(row=1, column=0)
entry_answer1 = tk.Entry(root, width=50)
entry_answer1.grid(row=1, column=1)

tk.Label(root, text="Réponse 2:").grid(row=2, column=0)
entry_answer2 = tk.Entry(root, width=50)
entry_answer2.grid(row=2, column=1)

tk.Label(root, text="Bonne Réponse:").grid(row=3, column=0)
correct_answer_var = tk.IntVar()
correct_answer_menu = tk.OptionMenu(root, correct_answer_var, 0, 1) # 0 pour la première réponse, 1 pour la seconde
correct_answer_menu.grid(row=3, column=1)

submit_button = tk.Button(root, text="Ajouter Question", command=submit)
submit_button.grid(row=4, column=1)

root.mainloop()
