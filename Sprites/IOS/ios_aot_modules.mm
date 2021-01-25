#include <mono/jit/jit.h>
#include "ios_aot_modules.h"
void ios_aot_register_modules()
{
  mono_aot_register_module((void **)mono_aot_module_Mono_Security_info);
  mono_aot_register_module((void **)mono_aot_module_System_Core_info);
  mono_aot_register_module((void **)mono_aot_module_System_Numerics_info);
  mono_aot_register_module((void **)mono_aot_module_System_Xml_info);
  mono_aot_register_module((void **)mono_aot_module_System_info);
  mono_aot_register_module((void **)mono_aot_module_UrhoDotNet_info);
  mono_aot_register_module((void **)mono_aot_module_mscorlib_info);
  mono_aot_register_module((void **)mono_aot_module_Game_info);
}
