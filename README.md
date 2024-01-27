### auteurs:
- Alexandre CHAPLAIS
- Julian DESCHARREAUX
- Tilio EBOR

# Projet-MAR

Jeu de parcours en 3D sur Unity.
POUR JOUER DEPUIS UNITY, IL FAUT LANCER DEPUIS LA SCENE MAIN MENU

## Player

Le player est un ours controllable avec les commandes suivantes :
- Z pour avancer, S pour reculer.
- Q pour tourner vers la gauche et D pour tourner vers la droite.
- Barre espace pour sauter.
- Shift pour sprinter.
- clique gauche pour attaquer.
- Echap pour revenir au menu.

La caméra suit le player en vue 3 ème personne. Et le player à 10 points de vie. 

## Boucle de jeu.

Au lancement on arrive sur le Menu pour lancer une partie.
La partie se déroule en deux parcours. Le Niveau 1 et le Niveau 2 qui offre différentes fonctionnalités.
Sauter à travers les obstacles, éviter les projectiles des tourelles et atteindre l'arrivée.
Le but étant d'aller au bout des 2 Niveaux pour gagner la partie.

## Tourelles et projectiles

Les tourelles lancent des projectiles enlevant chacun 1 point de vie.
Et peuvent être détruites avec l'attaque du player.
Lorsque le player n'a plus de points de vie il est ramené à l'écran des menus (défaite).

## Soin
Vous pouvez récuperer des points de vie (si vous n'etes pas au maximum) en récupérant les cubes de couleur rouge.
Soyez attentif ! Certains sont bien cachés =)

## Problemes / Bugs
Lorsque vous gagnez une partie, vous devez attendre 10 secondes avant d'avoir l'affichage du message de fin

Il arrive lorsque le saut se bloque lorsque nous avancons, il faut rappuyer sur la touche espace afin d'avoir un vrai saut.

Il peut arriver que vous soyez bloqué dans un élement de decor et que celui ci vous fasses passer sous la map, il faut dans ce cas relancer la partie.

### ASSETS
- TextMesh Pro
- Bridges Pack from Maxime Brunoni
- FREE Stylized Bear - RPG Forest Animal
- low poly pack
