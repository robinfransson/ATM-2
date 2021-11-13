import { Button, Card, Col, Form, Input, message, Row, Tooltip, Typography } from 'antd';
import React from 'react';
import { AtmContent, BillInfo } from '../Interfaces/ReturnTypes'
import { InfoCircleOutlined } from '@ant-design/icons';



const withdrawMoney = async (amount: number) => {
    const response = await fetch('/api/ATM', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(amount)
    })
    if (response.ok) {
        let json = await response.json() as AtmContent;
        return json;
    }
    else {
        message.error(await response.json());
        return undefined;
    }
}

const resetAtm = async () => {
    const response = await fetch('/api/ATM/reset', {
        method: 'GET'
    })
    if (response.ok) {
        return await initialLoad();
    }
    else {
        message.error(await response.json());
        return undefined;
    }
}

const initialLoad = async () => {
    const response = await fetch('/api/ATM', {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
    if (response.ok) {
        let json = await response.json() as AtmContent;
        return json;
    }
    else {
        //message.error(response.statusText);
        return undefined;
    }
}

export const Home = () => {


    const invalidChars = [
        "-",
        "+",
        "e",
    ];

    const [atmContents, setAtmContents] = React.useState<AtmContent>()
    const [form] = Form.useForm();
    const formRef = React.useRef<any>()

    React.useEffect(() => {
        const onload = async () => {
            let result = await initialLoad();
            setAtmContents(result);
            formRef.current.setFieldsValue({ withdrawalAmount: "0" })
        }
        onload();
    }, [])
    const reset = async () => {
        var result = await resetAtm();
        if (result) {
            setAtmContents(result);
            message.success("Successfully reset the ATM.", 3);
        }



    }

    const submit = async () => {
        let withdrawalAmount = formRef.current.getFieldValue("withdrawalAmount");
        let result: AtmContent | undefined;
        if (withdrawalAmount) {
            result = await withdrawMoney(withdrawalAmount)
        }
        if (result) {
            setAtmContents(result);
            message.success(`Successfully withdrew $${withdrawalAmount} from the ATM.`, 3)

            formRef.current.setFieldsValue({ withdrawalAmount: "" })
        }
        else if (!result && withdrawalAmount) {
            formRef.current.setFieldsValue({ withdrawalAmount: "" })
        }
        else {
            message.error("Input something first!", 3);
        }


    }


    return (
        <>
            <Row>
                <Col span={24} style={{ justifyContent: "center", alignContent: "center", display: "inline-flex" }}>
                    <Card title="ATM Machine" style={{ width: 300 }}>
                        {atmContents?.contents.map((bill: BillInfo, index: number) => <Typography.Paragraph key={index} style={{ textDecoration: "" }}>${bill.value} x {bill.amount}</Typography.Paragraph>)}
                        <Form form={form} ref={formRef}
                            onKeyDown={((e: any) => {
                                if (e.key === "Enter")
                                    submit()
                                else if (invalidChars.includes(e.key))
                                    e.preventDefault()
                            })}>
                            <Form.Item name="withdrawalAmount"
                                label={<>Amount to withdraw: <Tooltip title="Press enter to withdraw"><InfoCircleOutlined style={{ marginLeft: '3px' }} /></Tooltip></>}
                                labelCol={{ span: 24 }} style={{ borderTopWidth: '3px', borderTopColor: 'black', borderTopStyle: 'solid' }}>
                                <Input autoFocus={true} type="number" min={0} key="input-number"></Input>
                            </Form.Item>
                            <Button key="reset-button" onClick={reset}>Reset</Button>
                        </Form>
                    </Card>
                </Col>
            </Row>
        </>
    )
}

