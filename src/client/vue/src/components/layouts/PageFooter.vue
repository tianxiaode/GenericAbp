<template>
    <footer class="footer p-10 bg-primary text-white">
        <div class="container mx-auto flex justify-between items-center">
            <nav>
                <a href="/"><img :src="logo" style="height:54px;" alt="QuickTemplate" /></a>
                <div class="">2020 - {{ year }} © <a href="/">ExtJs.Tech</a></div>
                <p>
                    {{ t("nav.openSourceLicenseWith") }} <a
                        href="https://github.com/tianxiaode/QuickTemplate/blob/master/LICENSE">MIT</a>
                </p>
                <p><a href="https://beian.miit.gov.cn/">粤ICP备2022031812号</a></p>
            </nav>
            <nav class="flex items-start text-white flex-col gap-4">
                <a href="https://github.com/tianxiaode/QuickTemplate">{{ t("nav.github") }}</a>
                <a href="/about">{{ t("nav.about") }}</a>
                <a href="/docs">{{ t("nav.docs") }}</a>
            </nav>
            <nav class="flex items-center text-white flex-col gap-4">
                <a href="https://d.extjs.tech">{{ t("nav.desktop") }}</a>
                <a href="https://m.extjs.tech">{{ t("nav.mobile") }}</a>
            </nav>
        </div>
    </footer>

</template>

<script setup lang="ts">
import logo from "~/assets/logo.png";
import { useI18n } from "~/composables";

const year = new Date().getFullYear();
const { t } = useI18n();
</script>