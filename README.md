Šis projektas yra C# Windows Forms programa, kuri demonstruoja slaptažodžio atstatymą brute-force metodu.

## Funkcijos

- Atsitiktinio slaptažodžio sukūrimas
- Slaptažodžio hash naudojant SHA256
- Static salt naudojimas
- Brute-force paieška nuo ilgio 1 iki 6
- Single-thread brute-force
- Multi-thread brute-force
- Naudojama daugiausiai CPU cores - 1 gijų
- Start ir Stop mygtukai
- Progress bar
- Praėjusio laiko rodymas
- Rasto slaptažodžio išvedimas
- Single-thread ir multi-thread laiko palyginimas

## Klasės

- PasswordCreator.cs - sukuria atsitiktinį slaptažodį
- HashHelper.cs - skaičiuoja SHA256 hash su static salt
- PasswordValidator.cs - tikrina slaptažodį
- BruteForceGenerator.cs - generuoja galimas kombinacijas
- BruteForceSingle.cs - vykdo brute-force viena gija
- BruteForceMulti.cs - vykdo brute-force keliomis gijomis
- CrackResult.cs - saugo brute-force rezultatą
- MainForm.cs - grafinė vartotojo sąsaja

