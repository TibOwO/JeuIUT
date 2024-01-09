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
    load_questions()  # Rafraîchir la liste après l'ajout

def load_questions():
    listbox_questions.delete(0, tk.END)  # Effacer les entrées existantes
    conn = sqlite3.connect('Db_Unity.db')
    c = conn.cursor()
    c.execute("SELECT ID, STRING FROM QUESTIONS")
    for question in c.fetchall():
        question_text = f"Q: {question[1]}"
        listbox_questions.insert(tk.END, question_text)
        c2 = conn.cursor()
        c2.execute("SELECT STRING, IsCorrect FROM ANSWERS WHERE ParentID = ?", (question[0],))
        for answer in c2.fetchall():
            answer_text = f"  - {'[Correct]' if answer[1] == 1 else ''} R: {answer[0]}"
            listbox_questions.insert(tk.END, answer_text)
    conn.close()

def delete_question():
    selection = listbox_questions.curselection()
    if selection:
        conn = sqlite3.connect('Db_Unity.db')
        c = conn.cursor()
        question_text = listbox_questions.get(selection[0])
        question_id = question_text.split(':')[1].strip()
        c.execute("DELETE FROM ANSWERS WHERE ParentID = ?", (question_id,))
        c.execute("DELETE FROM QUESTIONS WHERE STRING = ?", (question_id,))
        conn.commit()
        conn.close()
        print(f"Question '{question_text}' supprimée de la base de données.")
        load_questions()  # Rafraîchir la liste après la suppression

def update_correct_answer_options():
    # Mettre à jour les options du menu déroulant avec les réponses actuelles
    menu = correct_answer_menu["menu"]
    menu.delete(0, "end")
    for answer in [entry_answer1.get(), entry_answer2.get()]:
        menu.add_command(label=answer, command=lambda value=answer: correct_answer_var.set(value))

    # Réinitialiser la sélection
    if entry_answer1.get() and entry_answer2.get():
        correct_answer_var.set(entry_answer1.get())

def correct_answer_menu_popup(event):
    update_correct_answer_options()

def submit():
    question = entry_question.get()
    answers = [entry_answer1.get(), entry_answer2.get()]
    correct_answer_text = correct_answer_var.get()

    # Trouver l'index de la bonne réponse
    try:
        correct_answer_index = answers.index(correct_answer_text)
        add_question_and_answers(question, answers, correct_answer_index)
    except ValueError:
        print("Erreur : La bonne réponse doit correspondre à l'une des réponses fournies.")

# Création de l'interface utilisateur
create_db()
root = tk.Tk()
root.title("Quiz Manager")
root.geometry("1000x600") # Taille de la fenêtre

tk.Label(root, text="Question:").grid(row=0, column=0)
entry_question = tk.Entry(root, width=50)
entry_question.grid(row=0, column=1)

tk.Label(root, text="Réponse 1:").grid(row=1, column=0)
entry_answer1 = tk.Entry(root, width=50)
entry_answer1.grid(row=1, column=1)
entry_answer1.bind('<FocusOut>', lambda e: update_correct_answer_options())


tk.Label(root, text="Réponse 2:").grid(row=2, column=0)
entry_answer2 = tk.Entry(root, width=50)
entry_answer2.grid(row=2, column=1)
entry_answer2.bind('<FocusOut>', lambda e: update_correct_answer_options())

tk.Label(root, text="Bonne Réponse:").grid(row=3, column=0)
correct_answer_var = tk.StringVar()
correct_answer_menu = tk.OptionMenu(root, correct_answer_var, "")
correct_answer_menu.grid(row=3, column=1)

correct_answer_menu.bind('<Button-1>', correct_answer_menu_popup)

submit_button = tk.Button(root, text="Ajouter Question", command=submit)
submit_button.grid(row=4, column=1)

# Ajout de la Listbox pour afficher les questions et les réponses
listbox_questions = tk.Listbox(root, width=50, height=10)
listbox_questions.grid(row=0, column=2, rowspan=4)

# Ajout d'un bouton pour supprimer une question
delete_button = tk.Button(root, text="Retirer Question", command=delete_question)
delete_button.grid(row=4, column=2)

load_questions()  # Charger les questions lors du démarrage

root.mainloop()
