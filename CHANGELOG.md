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
# 4.0.0 Created safe values for Ratio in TransformHandler
- The values for Ratio and Result, as well as the Rect from RectTransform, were broken down to improve understanding
- Safety measures were implemented to prevent NaN or +-Infinity from being returned for the Ratio and Result values. Therefore, the returned value will be Null in these cases.
# 5.0.0 Improvement
- Rename elements in the script for code improvement
- Correct rectorno guarantee for IsInstantiate
# 5.0.1 HotFix
- HotFix IsInstatiated property
# 5.0.2 Settings for operation in Editor Mode
- Some lines of code were changed and adjusted so Transform Handler can be used in editor mode without causing Null Reference errors related to camera access.
# 6.0.0 HotFix
- Fix for camera reference access in TransformHandler
- Fix and logic change in checking if AnimatorHandler needs to be instantiated
# 6.0.1 Code Improvement for Behavior
- Code improvement for running in editor mode