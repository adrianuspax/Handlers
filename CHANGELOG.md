# 0.0.0 First Commit
# 1.0.0 Add Files
# 1.0.1 Code improvement in Handler.Transform
- Added code so that the Handler.Transform behavior is updated in edit mode and Play mode of the Unity Editor
## Needs to be checked
- The ASPax attributes have lost effect due to the code update. It is necessary to check and correct, if possible and viable. If not, remove the unusable ASPax attributes.
# 2.0.0 Fixed loss of attribute effects and improvements
- Fixed loss of effects and manipulation of attributes in the Inspector related to the ASPax.Attributes library and its variables
- Changed the names of the "Animator" and "Transform" classes in the ASPax.Handler namespace to "AnimatorX" and "TransformX" so that the Unity console would no longer display the message about duplicate names, even though this in practice did not actually affect the use of transform by UnityEngine. (At least I had no problems!)
# 3.0.0 Information update and code improvement
- Created a more informative Readme document.
- Changed the name of the classes and the namespace.
- Created a boolean that returns if the AnimatorHandler class was instantiated
- Created a folder for inserting documents and images