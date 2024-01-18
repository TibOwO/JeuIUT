import tkinter as tk
import sqlite3
from tkinter import ttk


def create_db():
    conn = sqlite3.connect('Db_Unity.db')
    c = conn.cursor()
    # Modification pour inclure le champ 'Professeur'
    c.execute('''CREATE TABLE IF NOT EXISTS QUESTIONS (ID INTEGER PRIMARY KEY, STRING TEXT, Professeur TEXT)''')
    c.execute(
        '''CREATE TABLE IF NOT EXISTS ANSWERS (ID INTEGER PRIMARY KEY, STRING TEXT, ParentID INTEGER, IsCorrect INTEGER, Professeur TEXT, FOREIGN KEY(ParentID) REFERENCES QUESTIONS(ID))''')
    conn.commit()
    conn.close()


def add_question_and_answers(question, answers, correct_answer_index, professor):
    conn = sqlite3.connect('Db_Unity.db')
    c = conn.cursor()

    # Ajouter la question avec le champ 'Professeur'
    c.execute("INSERT INTO QUESTIONS (STRING, Professeur) VALUES (?, ?)", (question, professor))
    question_id = c.lastrowid

    # Ajouter les réponses avec le champ 'Professeur'
    for i, answer in enumerate(answers):
        is_correct = 1 if i == correct_answer_index else 0
        c.execute("INSERT INTO ANSWERS (STRING, ParentID, IsCorrect, Professeur) VALUES (?, ?, ?, ?)",
                  (answer, question_id, is_correct, professor))

    conn.commit()
    conn.close()
    print(
        f"Question ajoutée: '{question}' avec les réponses: {answers}. Bonne réponse: {answers[correct_answer_index]}")
    load_questions()  # Rafraîchir la liste après l'ajout


question_ids = []  # Liste globale pour stocker les IDs des questions


def load_questions():
    global question_ids
    question_ids.clear()
    tree.delete(*tree.get_children())  # Effacer les entrées existantes dans la Treeview
    conn = sqlite3.connect('Db_Unity.db')
    c = conn.cursor()
    c.execute("SELECT DISTINCT Professeur FROM QUESTIONS")
    profs = c.fetchall()

    # Création d'un nœud pour chaque professeur
    for prof in profs:
        prof_node = tree.insert('', 'end', text=prof[0], open=True)  # open=True pour ouvrir les nœuds par défaut
        c.execute("SELECT ID, STRING FROM QUESTIONS WHERE Professeur = ?", (prof[0],))
        for question in c.fetchall():
            question_id = question[0]
            question_text = question[1]
            question_node = tree.insert(prof_node, 'end', iid=question_id, text=question_text, tags=('question',))
            question_ids.append(question_id)  # Stocker l'ID correspondant
            c2 = conn.cursor()
            c2.execute("SELECT STRING, IsCorrect FROM ANSWERS WHERE ParentID = ?", (question_id,))
            for answer in c2.fetchall():
                answer_text = f"{'[Correct]' if answer[1] == 1 else ''} {answer[0]}"
                tree.insert(question_node, 'end', text=answer_text, tags=('answer',))
    conn.close()


def delete_question():
    selected_item = tree.selection()
    if selected_item:
        # Récupération de l'ID de l'élément sélectionné dans la Treeview
        question_id = selected_item[0]
        if not question_id.isdigit():
            # Si l'ID sélectionné n'est pas numérique, cela signifie que c'est une réponse, pas une question
            print("Veuillez sélectionner une question et non une réponse.")
            return

        question_id = int(question_id)
        conn = sqlite3.connect('Db_Unity.db')
        c = conn.cursor()
        # Supprimer les réponses d'abord
        c.execute("DELETE FROM ANSWERS WHERE ParentID = ?", (question_id,))
        # Ensuite, supprimer la question
        c.execute("DELETE FROM QUESTIONS WHERE ID = ?", (question_id,))
        conn.commit()
        conn.close()
        print(f"Question ID {question_id} supprimée de la base de données.")
        load_questions()  # Rafraîchir la Treeview après la suppression



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
    professor = entry_professor.get()
    # Trouver l'index de la bonne réponse
    try:
        correct_answer_index = answers.index(correct_answer_text)
        add_question_and_answers(question, answers, correct_answer_index, professor)
    except ValueError:
        print("Erreur : La bonne réponse doit correspondre à l'une des réponses fournies.")
    entry_question.delete(0, tk.END)
    entry_answer1.delete(0, tk.END)
    entry_answer2.delete(0, tk.END)
    correct_answer_var.set('')  # Réinitialiser le menu déroulant de la bonne réponse
    entry_professor.delete(0, tk.END)

create_db()
root = tk.Tk()
root.title("Quiz Manager")
root.state('zoomed') # Maximiser la fenêtre

input_frame = tk.Frame(root)
input_frame.grid(row=0, column=1, sticky='ew')

tk.Label(input_frame, text="Question:").grid(row=0, column=0)
entry_question = tk.Entry(input_frame, width=50)
entry_question.grid(row=0, column=1)

tk.Label(input_frame, text="Réponse 1:").grid(row=1, column=0)
entry_answer1 = tk.Entry(input_frame, width=50)
entry_answer1.grid(row=1, column=1)
entry_answer1.bind('<FocusOut>', lambda e: update_correct_answer_options())

tk.Label(input_frame, text="Réponse 2:").grid(row=2, column=0)
entry_answer2 = tk.Entry(input_frame, width=50)
entry_answer2.grid(row=2, column=1)
entry_answer2.bind('<FocusOut>', lambda e: update_correct_answer_options())

tk.Label(input_frame, text="Bonne Réponse:").grid(row=3, column=0)
correct_answer_var = tk.StringVar()
correct_answer_menu = tk.OptionMenu(input_frame, correct_answer_var, "")
correct_answer_menu.grid(row=3, column=1)
correct_answer_menu.bind('<Button-1>', correct_answer_menu_popup)

tk.Label(input_frame, text="Professeur:").grid(row=4, column=0)
entry_professor = tk.Entry(input_frame, width=50)
entry_professor.grid(row=4, column=1)

submit_button = tk.Button(input_frame, text="Ajouter Question", command=submit)
submit_button.grid(row=5, column=1)

# Création de la Treeview
tree = ttk.Treeview(root)
tree.grid(row=0, column=2, rowspan=5, sticky='nsew')  # Sticky pour l'expansion dans tous les sens

# Configuration pour permettre l'expansion de la colonne et de la rangée où se trouve la Treeview
root.grid_columnconfigure(2, weight=1)
root.grid_rowconfigure(0, weight=1)

# Ajouter des styles différents pour les questions et les réponses
style = ttk.Style()
style.configure("Treeview", font=('Helvetica', 12))
style.configure("Treeview.Heading", font=('Helvetica', 14, 'bold'))
style.configure('Question.Treeview', background='#E8E8E8')
style.configure('Answer.Treeview', background='#F5F5F5')

tree.tag_configure('question', font=('Helvetica', 12, 'bold'))
tree.tag_configure('answer', font=('Helvetica', 12))

tree.grid(row=0, column=2, rowspan=5, sticky='nsew')

delete_button = tk.Button(root, text="Retirer Question", command=delete_question)
delete_button.grid(row=5, column=2)

load_questions() # Charger les questions lors du démarrage

root.mainloop()



