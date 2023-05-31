#ifdef URHO3D_JS
#include "JavaScriptConsoleTab.h"
#include <Urho3D/SystemUI/Console.h>
#include <Urho3D/JavaScript/JavaScriptSystem.h>

#define UI_LINE_SIZE 30.0f
#define UI_MAX_RESIZE_RATIO 0.5f

namespace Urho3D
{
    void Foundation_JavaScriptConsoleTab(Context* context, Project* project)
    {
        project->AddTab(MakeShared<JavaScriptConsoleTab>(context));
    }

    JavaScriptConsoleTab::JavaScriptConsoleTab(Context* context)
        : EditorTab(context, "JavaScript Console", "18F16FD3-431B-4ACD-A6BE-78786E32E436",
            EditorTabFlag::None, EditorTabPlacement::DockBottom)
    {
    }

    void JavaScriptConsoleTab::RenderContent()
    {
        auto console = GetSubsystem<Console>();
        static float g_codeAreaHeight = 30;
        ImFont* font = Project::GetMonoFont();
        if (font)
            ui::PushFont(font);

        ImVec2 region = ui::GetContentRegionAvail();
        g_codeAreaHeight = Clamp(g_codeAreaHeight, UI_LINE_SIZE, region.y * UI_MAX_RESIZE_RATIO);

        if (ui::BeginChild("ConsoleArea", ImVec2(region.x, region.y - g_codeAreaHeight))) {
            console->RenderContent();
            ui::EndChild();
        }

        if (ui::InputTextMultiline("##jscode", &currentCode_, ImVec2(region.x, g_codeAreaHeight))) {
            g_codeAreaHeight = CalculateTextHeight();
        }

        if (ui::IsItemFocused() && ui::IsKeyDown(KEY_RETURN) && ui::IsKeyDown(KEY_LCTRL) && !currentCode_.empty())
        {
            GetSubsystem<JavaScriptSystem>()->Run(currentCode_);
            currentCode_.clear();
        }
        if (font)
            ui::PopFont();
    }

    float JavaScriptConsoleTab::CalculateTextHeight() {
        float height = 0.0f;
        for (unsigned i = 0; i < currentCode_.size(); ++i) {
            if (currentCode_.at(i) == '\n') {
                height += UI_LINE_SIZE;
            }
        }
        return height;
    }
}
#endif
