#include "JavaScriptOperations.h"
#include "JavaScriptMathResolvers.h"
#include "../IO/Log.h"
#include "../Container/Hash.h"

#define JS_OBJ_HIDDEN_EVENT_TABLE DUK_HIDDEN_SYMBOL("__events")
#define JS_BIND_HIDDEN_CALL_PROP DUK_HIDDEN_SYMBOL("__call")
#define JS_BIND_HIDDEN_TARGET_PROP DUK_HIDDEN_SYMBOL("__target")

namespace Urho3D
{
    StringHash g_ref_counted_type   = StringHash("RefCounted");
    StringHash g_boolean_type       = StringHash("bool");
    StringHash g_number_type        = StringHash("Number");
    StringHash g_int_type           = StringHash("int");
    StringHash g_uint_type          = StringHash("uint");
    StringHash g_float_type         = StringHash("float");
    StringHash g_double_type        = StringHash("double");
    StringHash g_buffer_type        = StringHash("Buffer");
    StringHash g_pointer_type       = StringHash("void");
    StringHash g_string_type        = StringHash("string");
    StringHash g_variant_map_type   = StringHash("VariantMap");
    StringHash g_variant_type       = StringHash("Variant");
    StringHash g_string_hash_type   = StringHash("StringHash");
    StringHash g_vector2_type       = StringHash("Vector2");
    StringHash g_vector3_type       = StringHash("Vector3");
    StringHash g_int_vector2_type   = StringHash("IntVector2");
    StringHash g_int_vector3_type   = StringHash("IntVector3");
    StringHash g_vector4_type       = StringHash("Vector4");
    StringHash g_quaternion_type    = StringHash("Quaternion");
    StringHash g_color_type         = StringHash("Color");
    StringHash g_rect_type          = StringHash("Rect");
    StringHash g_int_rect_type      = StringHash("IntRect");
    StringHash g_matrix2_type       = StringHash("Matrix2");
    StringHash g_matrix3_type       = StringHash("Matrix3");
    StringHash g_matrix3x4_type     = StringHash("Matrix3x4");
    StringHash g_matrix4_type       = StringHash("Matrix4");
    StringHash g_vector_type        = StringHash("Vector");

    StringHash rbfx_require_string_hash(duk_context* ctx, duk_idx_t stack_idx)
    {
        duk_int_t type = duk_get_type(ctx, stack_idx);
        StringHash result;
        if (type == DUK_TYPE_STRING)
            result = duk_get_string(ctx, stack_idx);
        else if (type == DUK_TYPE_NUMBER)
            result = StringHash(duk_get_uint(ctx, stack_idx));
        else {
            duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, "invalid StringHash type");
            duk_throw(ctx);
        }

        return result;
    }
    StringHash rbfx_get_string_hash(duk_context* ctx, duk_idx_t stack_idx)
    {
        duk_int_t type = duk_get_type(ctx, stack_idx);
        StringHash result;
        if (type == DUK_TYPE_STRING)
            result = duk_get_string(ctx, stack_idx);
        else if (type == DUK_TYPE_NUMBER)
            result = StringHash(duk_get_uint(ctx, stack_idx));
        return result;
    }
    void rbfx_push_string_hash(duk_context* ctx, const StringHash& hash) {
        duk_push_uint(ctx, hash.Value());
    }
    void rbfx_push_variant(duk_context* ctx, const Variant& vary)
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
                    rbfx_push_variant(ctx, varyList.at(i));
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
                rbfx_push_object(ctx, vary.GetPtr());
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

                    rbfx_push_variant(ctx, value);
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

                    rbfx_push_variant(ctx, value);
                    duk_put_prop_string(ctx, varMapIdx, key.c_str());
                }
            }
                break;
            default:
                duk_push_null(ctx);
                break;
        }
    }

    Variant rbfx_get_variant(duk_context* ctx, duk_idx_t stack_idx)
    {
        Variant vary;
        duk_idx_t type = duk_get_type(ctx, stack_idx);
        switch (type)
        {
        case DUK_TYPE_UNDEFINED:
        case DUK_TYPE_NULL:
            vary.Clear();
            break;
        case DUK_TYPE_BOOLEAN:
            vary = (bool)duk_get_boolean(ctx, stack_idx);
            break;
        case DUK_TYPE_NUMBER:
            vary = duk_get_number(ctx, stack_idx);
            break;
        case DUK_TYPE_STRING:
            vary = duk_get_string(ctx, stack_idx);
            break;
        case DUK_TYPE_OBJECT:
        {
            if (duk_get_prop_string(ctx, -1, "ptr")) {
                vary = static_cast<RefCounted*>(duk_get_pointer(ctx, -1));
                duk_pop(ctx);
            }
            else
            {
                duk_pop(ctx);
                if (duk_get_prop_string(ctx, -1, "type"))
                {
                    StringHash type = duk_get_string(ctx, -1);
                    duk_pop(ctx);

                    if (type == g_vector2_type)
                        vary = rbfx_vector2_resolve(ctx, stack_idx);
                    else if (type == g_int_vector2_type)
                        vary = rbfx_int_vector2_resolve(ctx, stack_idx);
                    else if (type == g_vector3_type)
                        vary = rbfx_vector3_resolve(ctx, stack_idx);
                    else if (type == g_vector4_type)
                        vary = rbfx_int_vector3_resolve(ctx, stack_idx);
                    else if (type == g_vector4_type)
                        vary = rbfx_vector4_resolve(ctx, stack_idx);
                    else if (type == g_quaternion_type)
                        vary = rbfx_quaternion_resolve(ctx, stack_idx);
                    else if (type == g_quaternion_type)
                        vary = rbfx_color_resolve(ctx, stack_idx);
                    else if (type == g_rect_type)
                        vary = rbfx_rect_resolve(ctx, stack_idx);
                    else if (type == g_int_rect_type)
                        vary = rbfx_int_rect_resolve(ctx, stack_idx);
                    else if (type == g_matrix2_type)
                    {
                        // unfortunally Variant does not support Matrix2
                        // in this case we use Vector4 instead
                        Matrix2 m = rbfx_matrix2_resolve(ctx, stack_idx);
                        vary = Vector4(m.m00_, m.m01_, m.m10_, m.m11_);
                    }
                    else if (type == g_matrix3_type)
                        vary = rbfx_matrix3_resolve(ctx, stack_idx);
                    else if (type == g_matrix3x4_type)
                        vary = rbfx_matrix3x4_resolve(ctx, stack_idx);
                    else if (type == g_matrix4_type)
                        vary = rbfx_matrix4_resolve(ctx, stack_idx);
                }
                else
                {
                    duk_pop(ctx);
                    vary = rbfx_get_variant_map(ctx, stack_idx);
                }
            }
        }
            break;
        case DUK_TYPE_BUFFER:
        {
            duk_size_t bufferSize = 0;
            void* buffer = duk_get_buffer(ctx, stack_idx, &bufferSize);

            VariantBuffer varyBuffer;
            varyBuffer.resize(bufferSize);
            memcpy(varyBuffer.data(), buffer, bufferSize);
            vary = varyBuffer;
        }
            break;
        case DUK_TYPE_POINTER:
            vary = duk_get_pointer(ctx, stack_idx);
            break;
        }
        return vary;
    }
    VariantMap rbfx_get_variant_map(duk_context* ctx, duk_idx_t stack_idx)
    {
        VariantMap result;
        duk_enum(ctx, stack_idx, 0);

        while (duk_next(ctx, stack_idx + 1, 1)) {
            const char* key = duk_get_string(ctx, -2);
            duk_idx_t value_idx = duk_get_top_index(ctx);
            result[key] = rbfx_get_variant(ctx, value_idx);
            duk_pop_2(ctx);
        }

        return result;
    }
    void rbfx_push_object(duk_context* ctx, RefCounted* ref_count)
    {
        if (!ref_count) {
            duk_push_null(ctx);
            return;
        }

        Object* obj = dynamic_cast<Object*>(ref_count);
        if (obj)
        {
            void* heapptr = obj->GetJSHeapptr();
            if (rbfx_is_valid_heapptr(ctx, heapptr))
            {
                duk_push_heapptr(ctx, heapptr);
            }
            // If object has not yet created, find for constructor and instantiate.
            else if(duk_get_global_string(ctx, obj->GetTypeName().c_str()))
            {
                duk_push_pointer(ctx, obj);
                duk_new(ctx, 1);
            }
            // If constructor has not been generated, return plain object instead.
            else
            {
                duk_idx_t obj_idx = duk_get_top(ctx);
                duk_push_object(ctx);

                rbfx_object_wrap(ctx, obj_idx, obj);
                rbfx_set_finalizer(ctx, obj_idx, obj);

                heapptr = duk_get_heapptr(ctx, -1);
                rbfx_lock_heapptr(ctx, heapptr);
                obj->SetJSHeapptr(heapptr);
                obj->AddRef();
            }
        }
        else
        {
            duk_idx_t obj_idx = duk_get_top(ctx);
            void* heapptr = ref_count->GetJSHeapptr();

            // If old wrap is alive on JS, return instead of create a new one
            if (rbfx_is_valid_heapptr(ctx, heapptr)) {
                duk_push_heapptr(ctx, heapptr);
                return;
            }
            
            duk_push_object(ctx);
            heapptr = duk_get_heapptr(ctx, obj_idx);
            ref_count->SetJSHeapptr(heapptr);
            ref_count->AddRef();

            rbfx_lock_heapptr(ctx, heapptr);
            rbfx_ref_counted_wrap(ctx, obj_idx, ref_count);
            rbfx_set_finalizer(ctx, obj_idx, ref_count);
        }
    }
    void rbfx_ref_counted_wrap(duk_context* ctx, duk_idx_t obj_idx, RefCounted* ref_count)
    {
        duk_push_pointer(ctx, ref_count);
        duk_put_prop_string(ctx, obj_idx, JS_OBJ_HIDDEN_PTR);
    }
    void rbfx_object_wrap(duk_context* ctx, duk_idx_t obj_idx, Object* obj)
    {
        rbfx_ref_counted_wrap(ctx, obj_idx, obj);
        duk_push_string(ctx, "type");
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_this(ctx);
            Object* instance = static_cast<Object*>(rbfx_get_instance(ctx, -1));
            duk_push_string(ctx, instance->GetTypeName().c_str());
            return 1;
        }, 1);
        duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
        // setup subscribeToEvent call
        duk_push_c_lightfunc(ctx, [](duk_context* ctx) {
            StringHash eventType = rbfx_require_string_hash(ctx, 0);
            duk_require_function(ctx, 1);

            duk_push_this(ctx);
            duk_get_prop_string(ctx, -1, JS_OBJ_HIDDEN_EVENT_TABLE);
            if (duk_get_prop_index(ctx, -1, eventType.Value()))
            {
                duk_dup(ctx, 1);
                // store callback into table. note: use callback address as key
                duk_put_prop_index(ctx, -2, (duk_uarridx_t)duk_get_heapptr(ctx, -1));
                return 0;
            }

            duk_pop(ctx);

            duk_push_object(ctx);
            duk_dup(ctx, 1);
            duk_put_prop_index(ctx, -2, (duk_uarridx_t)duk_get_heapptr(ctx, -1));
            duk_put_prop_index(ctx, -2, eventType.Value());

            duk_push_this(ctx);

            // listen to event
            Object* instance = static_cast<Object*>(rbfx_get_instance(ctx, duk_get_top(ctx) - 1));
            instance->SubscribeToEvent(eventType, [=](Object* obj, StringHash evtType, VariantMap& evtArgs) {
                ScriptEventCallDesc desc{
                    instance,
                    evtType,
                    evtArgs
                };
                rbfx_call_event(ctx, desc);
            });
            return 0;
        }, 2, 2, 0);
        duk_put_prop_string(ctx, obj_idx, "subscribeToEvent");
        // setup event table
        duk_push_object(ctx);
        duk_put_prop_string(ctx, obj_idx, JS_OBJ_HIDDEN_EVENT_TABLE);
    }
    RefCounted* rbfx_get_instance(duk_context* ctx, duk_idx_t obj_idx)
    {
        duk_get_prop_string(ctx, obj_idx, JS_OBJ_HIDDEN_PTR);
        void* ptr = duk_get_pointer_default(ctx, -1, nullptr);

        if (!ptr)
            return nullptr;
        return static_cast<RefCounted*>(ptr);
    }
    void rbfx_set_finalizer(duk_context* ctx, duk_idx_t obj_idx, RefCounted* ref_counted)
    {
        duk_push_c_function(ctx, [](duk_context* ctx) {
            // finalizer must release ref and free current heapptr to prevent issues
            duk_push_current_function(ctx);
            duk_get_prop_string(ctx, -1, JS_OBJ_HIDDEN_PTR);

            RefCounted* ptr = static_cast<RefCounted*>(duk_get_pointer(ctx, -1));
            rbfx_unlock_heapptr(ctx, ptr->GetJSHeapptr());
            ptr->SetJSHeapptr(nullptr);
            ptr->ReleaseRef();
            return 0;
        }, 0);
        // finalizers can't access push, in this case we must store
        // ptr instance into finalizer method.
        duk_push_pointer(ctx, ref_counted);
        duk_put_prop_string(ctx, -2, JS_OBJ_HIDDEN_PTR);

        duk_set_finalizer(ctx, obj_idx);
    }
    bool rbfx_is_valid_heapptr(duk_context* ctx, void* heapptr)
    {
        if (!heapptr) return false;

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_OBJECT_HEAPPTR_PROP);

        duk_bool_t result = duk_get_prop_index(ctx, -1, MakeHash(heapptr));
        duk_pop_3(ctx);

        return result;
    }
    void rbfx_lock_heapptr(duk_context* ctx, void* heapptr)
    {
        if (!heapptr) return;

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_OBJECT_HEAPPTR_PROP);

        duk_push_pointer(ctx, heapptr);
        duk_put_prop_index(ctx, -2, MakeHash(heapptr));

        duk_pop_2(ctx);
    }
    void rbfx_unlock_heapptr(duk_context* ctx, void* heapptr)
    {
        if (!heapptr) return;

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_OBJECT_HEAPPTR_PROP);
        duk_del_prop_index(ctx, -1, MakeHash(heapptr));

        duk_pop_2(ctx);
    }
    void rbfx_call_event(duk_context* ctx, const ScriptEventCallDesc& evtCall)
    {
        URHO3D_PROFILE("rbfx_call_event");
        if (!evtCall.instance_->GetJSHeapptr())
            return;

        duk_idx_t thisIdx = duk_get_top(ctx);
        duk_push_heapptr(ctx, evtCall.instance_->GetJSHeapptr());

        if (!duk_get_prop_string(ctx, -1, JS_OBJ_HIDDEN_EVENT_TABLE))
            return;

        duk_idx_t eventTableIdx = duk_get_top(ctx);
        if (!duk_get_prop_index(ctx, -1, evtCall.eventType_.Value()))
            return;


        duk_idx_t evtArgsIdx = duk_get_top(ctx);
        // push variant inside loop is slow, instead we just dup stack position into.
        rbfx_push_variant(ctx, evtCall.eventArgs_);

        duk_idx_t eventsIdx = duk_get_top(ctx);
        duk_enum(ctx, eventTableIdx, 0);
        while (duk_next(ctx, eventsIdx, 1))
        {
            // [this]
            duk_dup(ctx, thisIdx);
            // [this, event type]
            duk_push_uint(ctx, evtCall.eventType_.Value());
            // [this, event type, event args]
            duk_dup(ctx, evtArgsIdx);
            // final result must be like: this.eventCall(eventType, eventArgs)
            duk_call_method(ctx, 2);
        }
        // Reset stack, this prevent unexpected issues
        duk_pop_n(ctx, duk_get_top(ctx) - thisIdx);
    }
    void rbfx_make_strong(duk_context* ctx, void* heapptr)
    {
        if (!heapptr) return;

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_PROP_STRONG_REFS);
        duk_push_heapptr(ctx, heapptr);
        duk_put_prop_index(ctx, -2, (duk_uarridx_t)heapptr);
    }
    void rbfx_make_weak(duk_context* ctx, void* heapptr)
    {
        if (!heapptr) return;

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_PROP_STRONG_REFS);
        duk_push_null(ctx);
        duk_put_prop_index(ctx, -2, (duk_uarridx_t)heapptr);
    }

    void rbfx_bind(duk_context* ctx, duk_idx_t func_idx, duk_idx_t this_idx)
    {
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_idx_t argc = duk_get_top(ctx);
            duk_push_current_function(ctx);

            // get call and this bind
            duk_get_prop_string(ctx, -1, JS_BIND_HIDDEN_CALL_PROP);
            duk_get_prop_string(ctx, -2, JS_BIND_HIDDEN_TARGET_PROP);

            // duplicate arguments to top
            for (duk_idx_t i = 0; i < argc; ++i)
                duk_dup(ctx, i);

            duk_call_method(ctx, argc);
            return 1;
        }, DUK_VARARGS);

        duk_dup(ctx, func_idx);
        duk_put_prop_string(ctx, -2, JS_BIND_HIDDEN_CALL_PROP);

        duk_dup(ctx, this_idx);
        duk_put_prop_string(ctx, -2, JS_BIND_HIDDEN_TARGET_PROP);
    }

    void rbfx_try_catch(duk_context* ctx, duk_idx_t func_idx, duk_idx_t catch_call_idx)
    {
        duk_require_function(ctx, func_idx);
        duk_require_function(ctx, catch_call_idx);

        duk_eval_string(ctx, "(function(caller, reject) { try { caller(); } catch(e) { reject(e); } })");
        URHO3D_ASSERTLOG(duk_is_function(ctx, -1), "Evaluated try catch wrapper is not a function");

        duk_dup(ctx, func_idx);
        duk_dup(ctx, catch_call_idx);

        duk_call(ctx, 2);
    }

    StringHash rbfx_get_type(duk_context* ctx, duk_idx_t value_idx)
    {
        StringHash output(M_MAX_UNSIGNED);
        duk_idx_t type = duk_get_type(ctx, value_idx);

        if (duk_is_null_or_undefined(ctx, value_idx))
            return output;

        switch (type)
        {
            case DUK_TYPE_BOOLEAN:
                output = g_boolean_type;
                break;
            case DUK_TYPE_NUMBER:
                output = g_number_type;
                break;
            case DUK_TYPE_STRING:
                output = g_string_type;
                break;
            case DUK_TYPE_BUFFER:
                output = g_buffer_type;
                break;
            case DUK_TYPE_POINTER:
                output = g_pointer_type;
                break;
            case DUK_TYPE_OBJECT:
            {
                if (duk_is_array(ctx, value_idx))
                    output = g_vector_type;
                else
                {
                    if (duk_get_prop_string(ctx, value_idx, "type"))
                        output = duk_get_string(ctx, -1);
                    else
                        output = g_variant_map_type;
                    duk_pop(ctx);
                }
            }
                break;
        }

        return output;
    }
    void rbfx_require_array_type(duk_context* ctx, duk_idx_t value_idx, unsigned type_hash)
    {
        if (!duk_is_array(ctx, value_idx))
        {
            duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, "invalid value type. expected array(Iterable) like.");
            duk_throw(ctx);
            return;
        }

        duk_size_t len = duk_get_length(ctx, value_idx);
        if (len == 0)
            return;

        duk_get_prop_index(ctx, value_idx, 0);
        rbfx_require_type(ctx, value_idx, type_hash);
    }
    void rbfx_require_type(duk_context* ctx, duk_idx_t value_idx, unsigned type_hash)
    {
#define TYPE_REQ_ERROR_MSG(expectType) "invalid value type. the value type does not match type requirements or it's invalid. expected type: "#expectType
        // type validation can add a lot of boilerplate to final code that is not required.
        // for this reason, type validation will be removed on release builds
#ifdef URHO3D_DEBUG
        duk_idx_t type = duk_get_type(ctx, value_idx);
        ea::string err_output = "";

        // Variant is like any type of TypeScript.
        if (type_hash == g_variant_type.Value())
            return;

        if (type == DUK_TYPE_NULL || type == DUK_TYPE_UNDEFINED)
            err_output = "invalid value type. object specified is null.";
        else if (type_hash == g_string_hash_type.Value() && !(type == DUK_TYPE_STRING || type == DUK_TYPE_NUMBER))
            err_output = TYPE_REQ_ERROR_MSG(StringHash(string | Number));
        else if (type_hash == g_number_type.Value() && type != DUK_TYPE_NUMBER)
            err_output = TYPE_REQ_ERROR_MSG(Number);
        else if (type_hash == g_int_type.Value() && type != DUK_TYPE_NUMBER)
            err_output = TYPE_REQ_ERROR_MSG(int);
        else if (type_hash == g_uint_type.Value() && type != DUK_TYPE_NUMBER)
            err_output = TYPE_REQ_ERROR_MSG(uint);
        else if (type_hash == g_float_type.Value() && type != DUK_TYPE_NUMBER)
            err_output = TYPE_REQ_ERROR_MSG(float);
        else if (type_hash == g_double_type.Value() && type != DUK_TYPE_NUMBER)
            err_output = TYPE_REQ_ERROR_MSG(double);
        else if (type_hash == g_string_type.Value() && type != DUK_TYPE_STRING)
            err_output = TYPE_REQ_ERROR_MSG(string);
        else if (type_hash == g_buffer_type.Value() && type != DUK_TYPE_BUFFER)
            err_output = TYPE_REQ_ERROR_MSG(Buffer);
        else if (type_hash == g_pointer_type.Value() && type != DUK_TYPE_POINTER)
            err_output = TYPE_REQ_ERROR_MSG(ptr);
        else if (type_hash == g_variant_map_type.Value() && type != DUK_TYPE_OBJECT)
            err_output = TYPE_REQ_ERROR_MSG(VariantMap);
        else if (type == DUK_TYPE_OBJECT)
        {
            if (duk_get_prop_string(ctx, value_idx, "type"))
            {
                const char* type = duk_get_string(ctx, -1);
                StringHash obj_type_hash(type);
                Object* obj = static_cast<Object*>(rbfx_get_instance(ctx, value_idx));
                if (obj && !obj->GetTypeInfo()->IsTypeOf(StringHash(type_hash)))
                    err_output = "invalid value type. the value object type does not match type requirements.";
                else if (!obj && type_hash != obj_type_hash.Value())
                    err_output = "invalid value type. the value object type does not match type requirements.";
            }
            duk_pop(ctx);
        }

        if (!err_output.empty())
        {
            duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, err_output.c_str());
            duk_dup(ctx, value_idx);
            duk_put_prop_string(ctx, -2, "value");
            duk_throw(ctx);
        }
#endif
    }
}
