import sqlite3

# Nom de la base de données SQLite (changez-le selon vos besoins)
db_name = 'DB_Unity.db'

# Établir une connexion à la base de données
conn = sqlite3.connect(db_name)
cursor = conn.cursor()

# Script pour ajouter le champ "Professeur" à la table "ANSWERS"
cursor.execute("ALTER TABLE ANSWERS ADD COLUMN Professeur TEXT")

# Script pour ajouter le champ "Professeur" à la table "QUESTIONS"
cursor.execute("ALTER TABLE QUESTIONS ADD COLUMN Professeur TEXT")

# Valider les modifications
conn.commit()

# Fermer la connexion à la base de données
conn.close()

print("Les champs 'Professeur' ont été ajoutés aux tables 'ANSWERS' et 'QUESTIONS'.")
