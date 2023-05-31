#include "JavaScriptBindingUtils.h"

namespace Urho3D {
    StringHash require_string_hash(duk_context* ctx, duk_idx_t obj_idx)
    {
        if(duk_is_string(ctx, obj_idx))
			return duk_require_string(ctx, obj_idx);
        else
            return StringHash(duk_require_uint(ctx, obj_idx));
    }
    StringHash get_string_hash(duk_context* ctx, duk_idx_t obj_idx) 
    {
        StringHash result;
        if(duk_is_string(ctx, obj_idx))
            result = duk_get_string_default(ctx, obj_idx, "");
        else if(duk_is_number(ctx, 0))
            result = StringHash(duk_get_uint_default(ctx, obj_idx, 0u));
        return result;
    }
    Variant get_variant(duk_context* ctx, duk_idx_t obj_idx) 
    {
        Variant vary;
        return vary;
    }
    void push_variant(duk_context* ctx, const Variant& vary)
    {
        switch (vary.GetType())
            {
            case VAR_INT:
                duk_push_int(ctx, vary.GetInt());
                break;
            case VAR_INT64:
                duk_push_int(ctx, vary.GetInt64());
                break;
            case VAR_BOOL:
                duk_push_boolean(ctx, vary.GetBool());
                break;
            case VAR_FLOAT:
                duk_push_number(ctx, (duk_double_t)vary.GetFloat());
                break;
            case VAR_VECTOR2:
            {
                Vector2 vec = vary.GetVector2();
                duk_get_global_string(ctx, "Vector2");
                duk_push_number(ctx, vec.x_);
                duk_push_number(ctx, vec.y_);
                duk_new(ctx, 2);
            }
                break;
            case VAR_VECTOR3:
            {
                Vector3 vec = vary.GetVector3();
                duk_get_global_string(ctx, "Vector3");
                duk_push_number(ctx, vec.x_);
                duk_push_number(ctx, vec.y_);
                duk_push_number(ctx, vec.z_);
                duk_new(ctx, 3);
            }
                break;
            case VAR_VECTOR4:
            {
                Vector4 vec = vary.GetVector4();
                duk_get_global_string(ctx, "Vector4");
                duk_push_number(ctx, vec.x_);
                duk_push_number(ctx, vec.y_);
                duk_push_number(ctx, vec.z_);
                duk_push_number(ctx, vec.w_);
                duk_new(ctx, 4);
            }
                break;
            case VAR_QUATERNION:
            {
                Quaternion vec = vary.GetQuaternion();
                duk_get_global_string(ctx, "Quaternion");
                duk_push_number(ctx, vec.x_);
                duk_push_number(ctx, vec.y_);
                duk_push_number(ctx, vec.z_);
                duk_push_number(ctx, vec.w_);
                duk_new(ctx, 4);
            }
                break;
            case VAR_COLOR:
            {
                Color vec = vary.GetColor();
                duk_get_global_string(ctx, "Color");
                duk_push_number(ctx, vec.r_);
                duk_push_number(ctx, vec.g_);
                duk_push_number(ctx, vec.b_);
                duk_push_number(ctx, vec.a_);
                duk_new(ctx, 4);
            }
                break;
            case VAR_STRING:
                duk_push_string(ctx, vary.GetString().c_str());
                break;
            case VAR_BUFFER:
            {
                auto buffer = vary.GetBuffer();
                void* currBuff = duk_push_fixed_buffer(ctx, buffer.size());
                memcpy(currBuff, (void*)buffer[0], buffer.size());
            }
                break;
            case VAR_VOIDPTR:
                duk_push_pointer(ctx, vary.GetVoidPtr());
                break;
            case VAR_RESOURCEREF:
            {
                ResourceRef ref = vary.GetResourceRef();
                duk_get_global_string(ctx, "ResourceRef");
                duk_push_string(ctx, ref.name_.c_str());
                duk_push_uint(ctx, ref.type_.Value());
                duk_push_null(ctx);
            }
                break;
            case VAR_RESOURCEREFLIST:
            {
                auto refList = vary.GetResourceRefList();
                duk_get_global_string(ctx, "ResourceRefList");

                duk_push_array(ctx);
                for (duk_uarridx_t i = 0; i < refList.names_.size(); ++i) {
                    duk_push_string(ctx, refList.names_.at(i).c_str());
                    duk_put_prop_index(ctx, -2, i);
                }
                duk_push_uint(ctx, refList.type_.Value());

                duk_new(ctx, 2);
            }
                break;
            case VAR_VARIANTVECTOR:
            {
                duk_idx_t arrIdx = duk_get_top(ctx);
                duk_push_array(ctx);
                auto varyList = vary.GetVariantVector();
                for (duk_uarridx_t i = 0; i < varyList.size(); ++i) {
                    push_variant(ctx, varyList.at(i));
                    duk_put_prop_index(ctx, arrIdx, i);
                }
            }
                break;
            case VAR_INTRECT:
            {
                IntRect rect = vary.GetIntRect();
                duk_get_global_string(ctx, "IntRect");
                duk_push_int(ctx, rect.left_);
                duk_push_int(ctx, rect.top_);
                duk_push_int(ctx, rect.right_);
                duk_push_int(ctx, rect.bottom_);

                duk_new(ctx, 4);
            }
                break;
            case VAR_INTVECTOR2:
            {
                IntVector2 vec = vary.GetIntVector2();
                duk_get_global_string(ctx, "IntVector2");
                duk_push_int(ctx, vec.x_);
                duk_push_int(ctx, vec.y_);
                duk_new(ctx, 2);
            }
                break;
            case VAR_PTR:
                push_object(ctx, static_cast<Object*>(vary.GetPtr()));
                break;
            case VAR_MATRIX3:
            {
                Matrix3 matrix = vary.GetMatrix3();
                duk_get_global_string(ctx, "Matrix3");
                // First Column
                duk_push_number(ctx, matrix.m00_);
                duk_push_number(ctx, matrix.m01_);
                duk_push_number(ctx, matrix.m02_);
                // Second Column
                duk_push_number(ctx, matrix.m10_);
                duk_push_number(ctx, matrix.m11_);
                duk_push_number(ctx, matrix.m12_);
                // Third Column
                duk_push_number(ctx, matrix.m20_);
                duk_push_number(ctx, matrix.m21_);
                duk_push_number(ctx, matrix.m22_);
            }
                break;
            case VAR_MATRIX3X4:
            {
                Matrix3x4 matrix = vary.GetMatrix3x4();
                duk_get_global_string(ctx, "Matrix3x4");
                // First Column
                duk_push_number(ctx, matrix.m00_);
                duk_push_number(ctx, matrix.m01_);
                duk_push_number(ctx, matrix.m02_);
                duk_push_number(ctx, matrix.m03_);
                // Second Column
                duk_push_number(ctx, matrix.m10_);
                duk_push_number(ctx, matrix.m11_);
                duk_push_number(ctx, matrix.m12_);
                duk_push_number(ctx, matrix.m13_);
                // Third Column
                duk_push_number(ctx, matrix.m20_);
                duk_push_number(ctx, matrix.m21_);
                duk_push_number(ctx, matrix.m22_);
                duk_push_number(ctx, matrix.m23_);
            }
                break;
            case VAR_MATRIX4:
            {
                Matrix4 matrix = vary.GetMatrix4();
                duk_get_global_string(ctx, "Matrix4");
                // First Column
                duk_push_number(ctx, matrix.m00_);
                duk_push_number(ctx, matrix.m01_);
                duk_push_number(ctx, matrix.m02_);
                duk_push_number(ctx, matrix.m03_);
                // Second Column
                duk_push_number(ctx, matrix.m10_);
                duk_push_number(ctx, matrix.m11_);
                duk_push_number(ctx, matrix.m12_);
                duk_push_number(ctx, matrix.m13_);
                // Third Column
                duk_push_number(ctx, matrix.m20_);
                duk_push_number(ctx, matrix.m21_);
                duk_push_number(ctx, matrix.m22_);
                duk_push_number(ctx, matrix.m23_);
                // Fourth Column
                duk_push_number(ctx, matrix.m30_);
                duk_push_number(ctx, matrix.m31_);
                duk_push_number(ctx, matrix.m32_);
                duk_push_number(ctx, matrix.m33_);
            }
                break;
            case VAR_DOUBLE:
                duk_push_number(ctx, vary.GetDouble());
                break;
            case VAR_STRINGVECTOR:
            {
                auto strVec = vary.GetStringVector();
                duk_push_array(ctx);
                for (duk_uarridx_t i = 0; i < strVec.size(); ++i) {
                    duk_push_string(ctx, strVec[i].c_str());
                    duk_put_prop_index(ctx, -2, i);
                }
            }
                break;
            case VAR_RECT:
            {
                Rect rect = vary.GetRect();
                duk_get_global_string(ctx, "Rect");
                duk_push_number(ctx, rect.Left());
                duk_push_number(ctx, rect.Top());
                duk_push_number(ctx, rect.Right());
                duk_push_number(ctx, rect.Bottom());

                duk_new(ctx, 4);
            }
                break;
            case VAR_INTVECTOR3:
            {
                IntVector3 rect = vary.GetIntVector3();
                duk_get_global_string(ctx, "IntVector3");
                duk_push_int(ctx, rect.x_);
                duk_push_int(ctx, rect.y_);
                duk_push_int(ctx, rect.z_);

                duk_new(ctx, 3);
            }
                break;
            case VAR_VARIANTCURVE:
            {
                URHO3D_LOGWARNING("VariantCurve Variant is not supported.");
                duk_push_null(ctx);
            }
                break;
            case VAR_VARIANTMAP:
            {
                auto varMap = vary.GetVariantMap();
                duk_idx_t varMapIdx = duk_get_top(ctx);
                duk_push_object(ctx);
                for (auto it : varMap) {
                    StringHash key = it.first;
                    Variant value = it.second;

                    push_variant(ctx, value);
                    duk_put_prop_index(ctx, varMapIdx, key.Value());
                }
            }
                break;
            case VAR_STRINGVARIANTMAP:
            {
                auto varMap = vary.GetStringVariantMap();
                duk_idx_t varMapIdx = duk_get_top(ctx);
                duk_push_object(ctx);
                for (auto it : varMap) {
                    ea::string key = it.first;
                    Variant value = it.second;

                    push_variant(ctx, value);
                    duk_put_prop_string(ctx, varMapIdx, key.c_str());
                }
            }
                break;
            default:
                duk_push_null(ctx);
                break;
            }
    }

    Object* require_object(duk_context* ctx, duk_idx_t obj_idx) 
    {
        duk_get_prop_string(ctx, obj_idx, JS_OBJECT_PTR_PROP);
        Object* instance = static_cast<Object*>(duk_require_pointer(ctx, -1));
        duk_pop(ctx);
        
        return instance;
    }
    Object* require_object(duk_context* ctx, duk_idx_t obj_idx, StringHash expected_type) 
    {
        Object* obj = require_object(ctx, obj_idx);
        URHO3D_ASSERTLOG(obj->IsInstanceOf(expected_type), "Invalid InstanceOf object type.");
        return obj;
    }
    Object* get_object(duk_context* ctx, duk_idx_t obj_idx) 
    {
        duk_get_prop_string(ctx, obj_idx, JS_OBJECT_PTR_PROP);
        Object* instance = static_cast<Object*>(duk_get_pointer_default(ctx, -1, nullptr));
        duk_pop(ctx);
        return instance;
    }
    void push_object(duk_context* ctx, Object* instance) 
    {
        const ea::string& typeName = instance->GetTypeName();    
        if(!duk_get_global_string(ctx, typeName.c_str())) {
            duk_pop(ctx);
            duk_idx_t obj_idx = duk_get_top(ctx);
            duk_push_object(ctx);

            instance->AddRef();
            Object_ctor(ctx, obj_idx, instance);
            Object_Finalizer(ctx, obj_idx, instance);
            return;
        }
        duk_push_pointer(ctx, instance);
        duk_new(ctx, 1);
    }
    void push_primitive(duk_context* ctx, duk_idx_t args_count, const ea::string& primitive_name) {
        duk_idx_t top = duk_get_top(ctx);
        
        if(!duk_get_global_string(ctx, primitive_name.c_str())) {
            duk_pop(ctx);
            duk_push_object(ctx);
            return;
        }

        duk_idx_t argIdx = args_count;
        while(argIdx >= 0)
            duk_dup(ctx, top - argIdx);
        duk_new(ctx, args_count);
    }
    duk_bool_t push_safe_heapptr(duk_context* ctx, void* heapptr) {
        if(!heapptr) return false;

        duk_push_global_stash(ctx);
        if(!duk_get_prop_string(ctx, -1, JS_OBJECT_HEAPPTR_PROP)) {
            duk_pop_2(ctx);
            return false;
        }

        if(!duk_get_prop_index(ctx, -1, reinterpret_cast<duk_uarridx_t>(heapptr))) {
            duk_pop_3(ctx);
            return false;
        }

        duk_push_heapptr(ctx, heapptr);
        return true;
    }
    void lock_safe_heapptr(duk_context* ctx, void* heapptr) {
        if(!heapptr) return;

        duk_push_global_stash(ctx);
        if(!duk_get_prop_string(ctx, -1, JS_OBJECT_HEAPPTR_PROP)) {
            duk_pop(ctx);

            duk_push_object(ctx);
            duk_dup(ctx, -1);
            duk_put_prop_string(ctx, -3, JS_OBJECT_HEAPPTR_PROP);
        }

        duk_push_pointer(ctx, heapptr);
        duk_put_prop_index(ctx, -2, reinterpret_cast<duk_uarridx_t>(heapptr));
        duk_pop_2(ctx);
    }
    void unlock_safe_heapptr(duk_context* ctx, void* heapptr) {
        if(!heapptr) return;

        duk_push_global_stash(ctx);        
        if (!duk_get_prop_string(ctx, -1, JS_OBJECT_HEAPPTR_PROP)) {
            duk_pop_2(ctx);
            return;
        }

        duk_uarridx_t idx = reinterpret_cast<duk_uarridx_t>(heapptr);
        if (!duk_get_prop_index(ctx, -1, idx)) {
            duk_pop_3(ctx);
            return;
        }
        duk_pop(ctx);

        duk_del_prop_index(ctx, -1, idx);
        duk_pop_2(ctx);
    }
}