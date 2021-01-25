#ifndef IOS_AOT_MODULES_H
#define IOS_AOT_MODULES_H
 
extern "C" {
  extern void * mono_aot_module_Mono_Security_info;
  extern void * mono_aot_module_System_Core_info;
  extern void * mono_aot_module_System_Numerics_info;
  extern void * mono_aot_module_System_Xml_info;
  extern void * mono_aot_module_System_info;
  extern void * mono_aot_module_UrhoDotNet_info;
  extern void * mono_aot_module_mscorlib_info;
  extern void * mono_aot_module_Game_info;
} // extern C
 
void ios_aot_register_modules();
 
#endif
