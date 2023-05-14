#include <Urho3D/Engine/Application.h>
#include <Urho3D/IO/Log.h>

namespace Urho3D
{
    class JSPlayer : public Application {
        URHO3D_OBJECT(JSPlayer, Application);
    public:
        JSPlayer(Context* context);
        virtual void Setup() override;
        virtual void Start() override;
    };

}
URHO3D_DEFINE_APPLICATION_MAIN(Urho3D::JSPlayer);
