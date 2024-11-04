<template>
    <el-form-item
      v-if="item.wrapInFormItem !== false"
      :label="t(item.label)"
      :prop="item.prop"
      v-show="!item.condition || (typeof item.condition === 'function' ? item.condition(formData) : item.condition)"
    >
      <component
        :is="item.type"
        v-model="formData[item.prop]"
        v-bind="item.props"
      >
        <template v-if="item.type === ElCheckboxGroup || item.type === ElRadioGroup">
          <div v-bind="item.groupWrapProps">
            <el-checkbox v-for="option in item.options" :key="option.value" :label="option.value">{{ option.label }}</el-checkbox>
          </div>
        </template>
        <template v-if="item.type === ElSelect">
          <el-option v-for="option in item.options" :key="option.value" :label="option.label" :value="option.value"></el-option>
        </template>
        <template v-if="item.slot">
          <slot :name="item.slot" />
        </template>
        <custom-form-item
          v-if="item.items"
          v-for="subItem in item.items"
          :key="subItem.prop"
          :item="subItem"
          :form-data="formData"
        />
      </component>
    </el-form-item>
    <component
      v-else
      :is="item.type"
      v-model="formData[item.prop]"
      v-bind="item.props"
    >
      <template v-if="item.type === ElCheckboxGroup || item.type === ElRadioGroup">
        <div v-bind="item.groupWrapProps">        
          <el-checkbox v-for="option in item.options" :key="option.value" :label="option.value">{{ option.label }}</el-checkbox>
        </div>
      </template>
      <template v-if="item.type === ElSelect">
        <el-option v-for="option in item.options" :key="option.value" :label="option.label" :value="option.value"></el-option>
      </template>
      <template v-if="item.slot">
        <slot :name="item.slot" />
      </template>
      <FormItem
        v-if="item.items"
        v-for="subItem in item.items"
        :key="subItem.prop"
        :item="subItem"
        :form-data="formData"
      />
    </component>
</template>

<script setup lang="ts">
import { ElInput, ElCheckbox, ElCheckboxGroup, ElSwitch, ElSelect, ElDatePicker, ElTimePicker, ElSlider, ElRadioGroup, ElUpload, ElTabs, ElTabPane, ElCascader, ElColorPicker, ElInputNumber } from "element-plus";
import {  useI18n } from '~/composables';
type ComponentType =
    typeof ElInput
    | typeof ElCascader
    | typeof ElCheckbox
    | typeof ElCheckboxGroup
    | typeof ElSwitch
    | typeof ElSelect
    | typeof ElDatePicker
    | typeof ElTimePicker
    | typeof ElSlider
    | typeof ElRadioGroup
    | typeof ElUpload
    | typeof ElTabs
    | typeof ElTabPane
    | typeof ElColorPicker
    | typeof ElInputNumber
    | any; // 自定义组件

declare interface FormItemType {
    type: ComponentType; // 组件类型
    label?: string; // 标签
    prop: string; // 字段名
    placeholder?: string; // 占位符
    props?: any; // 组件属性
    items?: FormItemType[]; // 子项
    condition?: boolean | ((data: any) => boolean); // 条件渲染
    default?: any; // 默认值
    options?: any[]; // 选项列表，主要用于 select、radio-group 等
    slot?: any; // 插槽内容
    wrapInFormItem?: boolean; // 是否包裹在 FormItem 组件中
    groupWrapProps?: any //ElCheckboxGroup和ElRadioGroup的封装属性
}


const props = defineProps({
  item: {
    type: Object,
    required: true
  },
  formData: {
    type: Object,
    required: true
  }
});

const { t } = useI18n();
</script>

