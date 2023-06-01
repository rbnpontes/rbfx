#include "JavaScriptMathResolvers.h"

namespace Urho3D
{
    Vector2 rbfx_vector2_resolve(duk_context* ctx, duk_idx_t stack_idx)
    {
        Vector2 result;

        duk_get_prop_string(ctx, stack_idx, "x");
        result.x_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "y");
        result.y_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        return result;
    }
    Vector3 rbfx_vector3_resolve(duk_context* ctx, duk_idx_t stack_idx)
    {
        Vector3 result;

        duk_get_prop_string(ctx, stack_idx, "x");
        result.x_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "y");
        result.y_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "z");
        result.z_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        return result;
    }
    IntVector2 rbfx_int_vector2_resolve(duk_context* ctx, duk_idx_t stack_idx)
    {
        IntVector2 result;

        duk_get_prop_string(ctx, stack_idx, "x");
        result.x_ = duk_get_int(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "y");
        result.y_ = duk_get_int(ctx, -1);
        duk_pop(ctx);

        return result;
    }
    IntVector3 rbfx_int_vector3_resolve(duk_context* ctx, duk_idx_t stack_idx)
    {
        IntVector3 result;

        duk_get_prop_string(ctx, stack_idx, "x");
        result.x_ = duk_get_int(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "y");
        result.y_ = duk_get_int(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "z");
        result.z_ = duk_get_int(ctx, -1);
        duk_pop(ctx);

        return result;
    }
    Vector4 rbfx_vector4_resolve(duk_context* ctx, duk_idx_t stack_idx)
    {
        Vector4 result;

        duk_get_prop_string(ctx, stack_idx, "x");
        result.x_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "y");
        result.y_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "z");
        result.z_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "w");
        result.w_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        return result;
    }
    Quaternion rbfx_quaternion_resolve(duk_context* ctx, duk_idx_t stack_idx)
    {
        Quaternion result;

        duk_get_prop_string(ctx, stack_idx, "x");
        result.x_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "y");
        result.y_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "z");
        result.z_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "w");
        result.w_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        return result;
    }
    Color rbfx_color_resolve(duk_context* ctx, duk_idx_t stack_idx)
    {
        Color result;

        duk_get_prop_string(ctx, stack_idx, "r");
        result.r_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "g");
        result.g_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "b");
        result.b_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "a");
        result.a_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        return result;
    }
    Rect rbfx_rect_resolve(duk_context* ctx, duk_idx_t stack_idx)
    {
        Rect result;

        duk_get_prop_string(ctx, stack_idx, "left");
        result.min_.x_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "top");
        result.min_.y_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "right");
        result.max_.x_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "bottom");
        result.max_.y_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        return result;
    }
    IntRect rbfx_int_rect_resolve(duk_context* ctx, duk_idx_t stack_idx)
    {
        IntRect result;

        duk_get_prop_string(ctx, stack_idx, "left");
        result.left_ = duk_get_int(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "top");
        result.top_ = duk_get_int(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "right");
        result.right_ = duk_get_int(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "bottom");
        result.bottom_ = duk_get_int(ctx, -1);
        duk_pop(ctx);

        return result;
    }
    Matrix2 rbfx_matrix2_resolve(duk_context* ctx, duk_idx_t stack_idx)
    {
        Matrix2 result;

        // first row
        duk_get_prop_string(ctx, stack_idx, "m00");
        result.m00_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m01");
        result.m01_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);
        // second row
        duk_get_prop_string(ctx, stack_idx, "m10");
        result.m10_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m11");
        result.m11_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        return result;
    }
    Matrix3 rbfx_matrix3_resolve(duk_context* ctx, duk_idx_t stack_idx)
    {
        Matrix3 result;

        // first row
        duk_get_prop_string(ctx, stack_idx, "m00");
        result.m00_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m01");
        result.m01_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m02");
        result.m02_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);
        // second Row
        duk_get_prop_string(ctx, stack_idx, "m10");
        result.m10_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m11");
        result.m11_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m12");
        result.m12_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);
        // third row
        duk_get_prop_string(ctx, stack_idx, "m20");
        result.m20_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m21");
        result.m21_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m22");
        result.m22_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);
        return result;
    }
    Matrix3x4 rbfx_matrix3x4_resolve(duk_context* ctx, duk_idx_t stack_idx)
    {
        Matrix3x4 result;

        // first row
        duk_get_prop_string(ctx, stack_idx, "m00");
        result.m00_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m01");
        result.m01_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m02");
        result.m02_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m03");
        result.m03_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);
        // second Row
        duk_get_prop_string(ctx, stack_idx, "m10");
        result.m10_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m11");
        result.m11_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m12");
        result.m12_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m13");
        result.m13_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);
        // third row
        duk_get_prop_string(ctx, stack_idx, "m20");
        result.m20_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m21");
        result.m21_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m22");
        result.m22_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m23");
        result.m23_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);
        return result;
    }
    Matrix4 rbfx_matrix4_resolve(duk_context* ctx, duk_idx_t stack_idx)
    {
        Matrix4 result;

        // first row
        duk_get_prop_string(ctx, stack_idx, "m00");
        result.m00_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m01");
        result.m01_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m02");
        result.m02_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m03");
        result.m03_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);
        // second Row
        duk_get_prop_string(ctx, stack_idx, "m10");
        result.m10_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m11");
        result.m11_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m12");
        result.m12_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m12");
        result.m13_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);
        // third row
        duk_get_prop_string(ctx, stack_idx, "m20");
        result.m20_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m21");
        result.m21_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m22");
        result.m22_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m22");
        result.m23_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);
        // fourth row
        duk_get_prop_string(ctx, stack_idx, "m30");
        result.m30_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m31");
        result.m31_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m32");
        result.m32_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        duk_get_prop_string(ctx, stack_idx, "m32");
        result.m33_ = (float)duk_get_number(ctx, -1);
        duk_pop(ctx);

        return result;
    }
}
